using AutoMapper;
using Method635.App.BL.BusinessServices.BrainstormingStateMachine;
using Method635.App.BL.Context;
using Method635.App.BL.Interfaces;
using Method635.App.Dal.Interfaces;
using Method635.App.Logging;
using Method635.App.Models;
using Method635.App.Models.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Method635.App.BL
{
    public class BrainstormingService : PropertyChangedBase, IBrainstormingService
    {
        private readonly BrainstormingContext _context;
        private readonly IMapper _mapper;
        private readonly IBrainstormingDalService _brainstormingDalService;
        private readonly ITeamDalService _teamDalService;
        private readonly IFileDalService _fileDalService;
        private readonly IPatternDalService _patternDalService;
        private readonly StateMachine _stateMachine;
        private int commitIdeaIndex = 0;
        private readonly BrainstormingModel _brainstormingModel;

        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public BrainstormingService(
            IDalService iDalService,
            IMapper mapper,
            BrainstormingContext brainstormingContext,
            BrainstormingModel brainstormingModel)
        {
            _context = brainstormingContext;
            _mapper = mapper;
            _brainstormingDalService = iDalService.BrainstormingDalService;
            _teamDalService = iDalService.TeamDalService;
            _fileDalService = iDalService.FileDalService;
            _patternDalService = iDalService.PatternDalService;
            _stateMachine = new StateMachine(_brainstormingDalService, _context, brainstormingModel);
            _stateMachine.PropertyChanged += StateMachine_PropertyChanged;

            _brainstormingModel = brainstormingModel;
            _brainstormingModel.PropertyChanged += _brainstormingModel_PropertyChanged;
        }

        private void _brainstormingModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            BrainWaveSent = _brainstormingModel.BrainWaveSent;
            BrainSheets = _brainstormingModel.BrainSheets;
            RemainingTime = _brainstormingModel.RemainingTime;
            CurrentSheetIndex = _brainstormingModel.CurrentSheetIndex;
        }

        public void StartBusinessService()
        {
            IsModerator = _teamDalService.GetModeratorByTeamId(_context.CurrentBrainstormingTeam?.Id)?.UserName.Equals(_context.CurrentParticipant?.UserName);

            _stateMachine.Start();
        }

        public void StopBusinessService()
        {
            _stateMachine.Stop();
            _stateMachine.PropertyChanged -= StateMachine_PropertyChanged;
            _brainstormingModel.PropertyChanged -= _brainstormingModel_PropertyChanged;
        }

        private void StateMachine_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsWaiting = _stateMachine.CurrentState is WaitingState;
            IsRunning = _stateMachine.CurrentState is RunningState;
            IsEnded = _stateMachine.CurrentState is EndedState;
        }


        public void SendBrainWave()
        {
            if (_context.CurrentFinding.BrainSheets == null)
            {
                _logger.Error("Brainsheets were null, can't send brainwave!");
                throw new ArgumentException("Brainsheets on current finding can't be null");
            }
            try
            {
                var nrOfBrainsheets = _context.CurrentFinding.BrainSheets.Count;
                var currentSheet = _context.CurrentFinding.BrainSheets[(_context.CurrentFinding.CurrentRound + _positionInTeam - 1) % nrOfBrainsheets];
                if (!_brainstormingDalService.UpdateSheet(_context.CurrentFinding.Id, currentSheet))
                {
                    _logger.Error("Couldn't place brainsheet");
                }
                _brainstormingModel.BrainWaveSent = true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.Error("Invalid index access!", ex);
            }
        }
        public void StartBrainstorming()
        {
            _brainstormingDalService.StartBrainstormingFinding(_context.CurrentFinding.Id);
            _context.CurrentFinding = _brainstormingDalService.GetFinding(_context.CurrentFinding.Id);
        }

        public async Task CommitIdea(Idea idea)
        {
            try
            {
                _brainstormingModel.BrainWaves[_context.CurrentFinding.CurrentRound - 1]
                    .Ideas[commitIdeaIndex % _context.CurrentFinding.NrOfIdeas] = idea;
                commitIdeaIndex++;

                if(idea is PictureIdea pictureIdea)
                {
                    await SetPictureImageSource(pictureIdea);
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.Error("Invalid index access!", ex);
            }
        }

        public async Task SetPictureImageSource(Idea idea)
        {
            if (!(idea is PictureIdea pictureIdea))
                return;

            var stream = await DownloadPictureIdea(pictureIdea);
            if (stream == null) return;
            var bytes = ConvertToBytes(stream);
            pictureIdea.ImageSource = ImageSource.FromFile(CacheImageBytesToFile(bytes, pictureIdea.PictureId));
        }

        private byte[] ConvertToBytes(Stream stream)
        {
            var memStream = new MemoryStream();
            stream.CopyTo(memStream);
            byte[] bytes = memStream.ToArray();
            memStream.Dispose();
            stream.Dispose();
            return bytes;
        }

        public void UploadSketchIdea(SketchIdea sketchIdea, byte[] imageBytes)
        {
            var stream = new MemoryStream(imageBytes);
            var fileId = _fileDalService.UploadFile(stream);
            stream.Dispose();
            sketchIdea.PictureId = fileId;
            sketchIdea.ImageSource = ImageSource.FromFile(CacheImageBytesToFile(imageBytes, sketchIdea.PictureId));
        }

        public List<PatternIdea> DownloadPatternIdeas()
        {
            return _patternDalService.GetAllPatterns();
        }

        private async Task<Stream> DownloadPictureIdea(PictureIdea pictureIdea)
        {
            return await Task.Run(() => _fileDalService.Download(pictureIdea.PictureId));
        }

        private string CacheImageBytesToFile(byte[] imageBytes, string pictureId)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{pictureId}.png");

            if (File.Exists(fileName))
                return fileName;

            var imageStream = new MemoryStream(imageBytes);
            using (var fs = File.Create(fileName))
            {
                imageStream.CopyTo(fs);
                imageStream.Dispose();
            }
            return fileName;
        }

        private bool _isWaiting;
        public bool IsWaiting
        {
            get => _isWaiting;
            set => SetProperty(ref _isWaiting, value);
        }
        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private bool _isEnded;
        public bool IsEnded
        {
            get => _isEnded;
            set => SetProperty(ref _isEnded, value);
        }


        private bool _brainWaveSent;
        public bool BrainWaveSent
        {
            get => _brainWaveSent;
            set => SetProperty(ref _brainWaveSent, value);
        }

        private ObservableCollection<BrainSheet> _brainSheets;
        public ObservableCollection<BrainSheet> BrainSheets { get => _brainSheets; set => SetProperty(ref _brainSheets, value); }
        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            set => SetProperty(ref _remainingTime, value);
        }

        public bool? IsModerator { get; private set; }

        private int _positionInTeam => _teamParticipants.IndexOf(_teamParticipants.Find(p => p.UserName.Equals(_context.CurrentParticipant.UserName)));
        private List<Participant> _teamParticipants => _context.CurrentBrainstormingTeam.Participants;

        private int _currentSheetNr;

        public int CurrentSheetIndex
        {
            get => _currentSheetNr;
            set => SetProperty(ref _currentSheetNr, value);
        }
    }
}
