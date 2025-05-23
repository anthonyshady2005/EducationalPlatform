﻿using System;
using System.Collections.Generic;

namespace WebAppDB.Models;

public partial class LearnersMastery
{
    public int LearnerId { get; set; }

    public int QuestId { get; set; }

    public string? Skill { get; set; }

    public string? CompletionStatus { get; set; }

    public virtual Learner Learner { get; set; } = null!;

    public virtual SkillMastery? SkillMastery { get; set; }
}
