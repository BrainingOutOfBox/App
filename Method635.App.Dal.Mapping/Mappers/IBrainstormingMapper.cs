﻿using Method635.App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Dal.Mapping.Mappers
{
    public interface IBrainstormingMapper
    {
        BrainstormingFinding MapFromDto(BrainstormingFindingDto brainstormingFindingDto);
        BrainstormingFindingDto MapToDto(BrainstormingFinding brainstormingFinding);
        List<BrainstormingFinding> MapFromDto(List<BrainstormingFindingDto> brainstormingFindingDtos);
        List<BrainstormingFindingDto> MapToDto(List<BrainstormingFinding> brainstormingFindings);
        BrainSheet MapFromDto(BrainSheetDto brainSheetDto);
        BrainSheetDto MapToDto(BrainSheet brainSheet);
    }
}
