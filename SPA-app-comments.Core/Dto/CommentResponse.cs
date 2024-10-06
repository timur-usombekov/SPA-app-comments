﻿using SPA_app_comments.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA_app_comments.Core.Dto
{
    public class CommentResponse
    {
        public Guid Id { get; set; }
        public Guid? ParentCommentId { get; set; }
        public UserResponse User { get; set; }
        public string Text { get; set; } = null!;
        public byte[]? File { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime CreatedAt { get; set; }


        //public List<CommentResponse> Replies { get; set; }
    }
}
