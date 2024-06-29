using CandidateAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace CandidateAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Candidate> Candidates { get; set; }
    }
}