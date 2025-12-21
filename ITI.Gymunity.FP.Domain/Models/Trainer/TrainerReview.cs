using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.Trainer
{
    public class TrainerReview : BaseEntity
    {
        // main relationships
        public int TrainerId { get; set; }
        public TrainerProfile Trainer { get; set; } = null!;

        public int ClientId { get; set; }
        public ClientProfile Client { get; set; } = null!;

        // بيانات التقييم
        public int Rating { get; set; }          // 1 → 5
        public string? Comment { get; set; }     // تعليق العميل

        // تحسينات
        public bool IsEdited { get; set; }
        public DateTimeOffset? EditedAt { get; set; }

        public bool IsApproved { get; set; }
        public DateTimeOffset? ApprovedAt { get; set; }


    }
}
