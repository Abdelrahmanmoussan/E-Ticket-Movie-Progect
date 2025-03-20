﻿using Microsoft.EntityFrameworkCore;

namespace E_Ticket.Models
{
    [PrimaryKey(nameof(MovieId), nameof(ApplicationUserId))]
    public class Cart
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int Count { get; set; }
    }
}
