using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace projecttour.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<CategoryBlog> CategoryBlogs { get; set; }

    public virtual DbSet<CategoryTour> CategoryTours { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<DepartureSchedule> DepartureSchedules { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TourDetail> TourDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-HKR8KJF\\MSSQLSERVER01;Database=KarnelTravelGuide;user id=sa;password=12345678;trusted_connection=true;encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("BLogs");

            entity.Property(e => e.BlogId).HasColumnName("Blog_ID");
            entity.Property(e => e.CategoryBlogId).HasColumnName("Category_Blog_ID");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.CategoryBlog).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryBlogId)
                .HasConstraintName("FK_BLogs_Category_BLogs");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(e => e.BookingId)
                .ValueGeneratedNever()
                .HasColumnName("Booking_ID");
            entity.Property(e => e.BookingDate)
                .HasColumnType("date")
                .HasColumnName("Booking_Date");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Contact_Number");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Customer_Email");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Customer_Name");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.NumberOfAdults).HasColumnName("Number_of_Adults");
            entity.Property(e => e.NumberOfChildren).HasColumnName("Number of Children");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("date")
                .HasColumnName("Payment_Date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Payment_Method");
            entity.Property(e => e.StatusPayment).HasColumnName("Status_Payment");
            entity.Property(e => e.TotalPrice).HasColumnName("Total_Price");
            entity.Property(e => e.TourId).HasColumnName("Tour_ID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Bookings_Users");

            entity.HasOne(d => d.Tour).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK_Bookings_Tours");
        });

        modelBuilder.Entity<CategoryBlog>(entity =>
        {
            entity.ToTable("Category_BLogs");

            entity.Property(e => e.CategoryBlogId).HasColumnName("Category_Blog_ID");
            entity.Property(e => e.CategoryBlogName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Category_BLog_Name");
        });

        modelBuilder.Entity<CategoryTour>(entity =>
        {
            entity.HasKey(e => e.CategoryTourId).HasName("PK_Categories");

            entity.ToTable("Category_Tours");

            entity.Property(e => e.CategoryTourId).HasColumnName("Category_Tour_ID");
            entity.Property(e => e.CategoryTourName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Category_Tour_Name");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.Property(e => e.ContactId).HasColumnName("Contact_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Full_Name");
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Phone_Number");
        });

        modelBuilder.Entity<DepartureSchedule>(entity =>
        {
            entity.HasKey(e => e.DepartureScheduleId).HasName("PK_Tour_Can");

            entity.ToTable("Departure_Schedules");

            entity.Property(e => e.DepartureScheduleId).HasColumnName("Departure_Schedule_ID");
            entity.Property(e => e.DepartureDate)
                .HasColumnType("date")
                .HasColumnName("Departure_Date");
            entity.Property(e => e.EmptySeat).HasColumnName("Empty_Seat");
            entity.Property(e => e.Standard)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TourId).HasColumnName("Tour_ID");

            entity.HasOne(d => d.Tour).WithMany(p => p.DepartureSchedules)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK_Departure_Schedules_Tours");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.Property(e => e.ReviewId).HasColumnName("Review_ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Full_Name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.Rating)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TourId).HasColumnName("Tour_ID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Reviews_Users");

            entity.HasOne(d => d.Tour).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK_Reviews_Tours");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.TourId).HasName("PK_Tour_Details");

            entity.Property(e => e.TourId).HasColumnName("Tour_ID");
            entity.Property(e => e.CategoryTourId).HasColumnName("Category_Tour_ID");
            entity.Property(e => e.CategoryTourName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DepartureDate)
                .HasColumnType("date")
                .HasColumnName("Departure_Date");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Duration)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmptySeat).HasColumnName("Empty_Seat");
            entity.Property(e => e.Hotel)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PointOfDeparture)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Point_Of_Departure");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TourType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Tour Type");
            entity.Property(e => e.Tranfer)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CategoryTour).WithMany(p => p.Tours)
                .HasForeignKey(d => d.CategoryTourId)
                .HasConstraintName("FK_Tours_Category_Tours");
        });

        modelBuilder.Entity<TourDetail>(entity =>
        {
            entity.HasKey(e => e.TourDetailId).HasName("PK_Tour_Details_1");

            entity.ToTable("Tour_Details");

            entity.Property(e => e.TourDetailId).HasColumnName("Tour_Detail_ID");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.Header)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Tiltle)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TourId).HasColumnName("Tour_ID");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourDetails)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK_Tour_Details_Tours");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("User_ID");
            entity.Property(e => e.Desciption)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Full_Name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.Salt).HasMaxLength(8);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
