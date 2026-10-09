using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<DetailReservation> DetailReservations { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("Users");
                user.HasKey(user => user.Id);

                user.Property(user => user.Id)
                    .ValueGeneratedOnAdd();

                user.Property(user => user.Name)
                    .IsRequired();

                user.Property(user => user.LastName)
                    .IsRequired();

                user.Property(user => user.Mail)
                    .IsRequired();

                user.Property(user => user.Password)
                    .IsRequired();

                user.Property(user => user.Dni)
                    .IsRequired();

                user.Property(user => user.Phone)
                    .IsRequired();

                user.Property(user => user.Type)
                    .IsRequired();

                user.HasMany(user => user.Reservations)
                    .WithOne(reservation => reservation.User)
                    .HasForeignKey(reservation => reservation.UserId);
            });

            modelBuilder.Entity<Reservation>(reservation =>
            {
                reservation.ToTable("Reservation");
                reservation.HasKey(reservation => reservation.Id);

                reservation.Property(reservation => reservation.Id)
                    .ValueGeneratedOnAdd();

                reservation.Property(reservation => reservation.ReservationCode)
                    .IsRequired();

                reservation.Property(reservation => reservation.CreationDate)
                    .IsRequired();

                reservation.Property(reservation => reservation.StartDate)
                    .IsRequired();

                reservation.Property(reservation => reservation.EndDate)
                    .IsRequired();

                reservation.Property(reservation => reservation.Type)
                    .IsRequired();

                reservation.Property(reservation => reservation.TotalAmount)
                    .IsRequired();

                reservation.Property(reservation => reservation.UserId)
                    .IsRequired();

                reservation.HasOne(reservation => reservation.User)
                    .WithMany(user => user.Reservations)
                    .HasForeignKey(reservation => reservation.UserId);

                reservation.HasMany(reservation => reservation.DetailReservations)
                    .WithOne(detail => detail.Reservation)
                    .HasForeignKey(detail => detail.ReservationId);

                reservation.HasMany(reservation => reservation.Payments)
                    .WithOne(payment => payment.Reservation)
                    .HasForeignKey(payment => payment.ReservationId);
            });

            modelBuilder.Entity<Payment>(payment =>
            {
                payment.ToTable("Payment");
                payment.HasKey(payment => payment.Id);

                payment.Property(payment => payment.Id)
                    .ValueGeneratedOnAdd();

                payment.Property(payment => payment.TransactionNumber)
                    .IsRequired();

                payment.Property(payment => payment.Amount)
                    .IsRequired();

                payment.Property(payment => payment.PaymentMethod)
                    .IsRequired();

                payment.Property(payment => payment.PaymentState)
                    .IsRequired();

                payment.Property(payment => payment.PaymentDate)
                    .IsRequired();

                payment.Property(payment => payment.ReservationId)
                    .IsRequired();

                payment.HasOne(payment => payment.Reservation)
                    .WithMany(reservation => reservation.Payments)
                    .HasForeignKey(payment => payment.ReservationId);
            });

            modelBuilder.Entity<Room>(room =>
            {
                room.ToTable("Room");
                room.HasKey(room => room.Id);

                room.Property(room => room.Id)
                    .ValueGeneratedOnAdd();

                room.Property(room => room.Number)
                    .IsRequired();

                room.Property(room => room.Type)
                    .IsRequired();

                room.Property(room => room.PricePerNight)
                    .IsRequired();

                room.Property(room => room.State)
                    .IsRequired();

                room.Property(room => room.Capacity)
                    .IsRequired();

                room.HasMany(room => room.DetailReservations)
                    .WithOne(detail => detail.Room)
                    .HasForeignKey(detail => detail.RoomId);
            });

            modelBuilder.Entity<DetailReservation>(detail =>
            {
                detail.ToTable("DetailReservation");
                detail.HasKey(detail => detail.Id);

                detail.Property(detail => detail.Id)
                    .ValueGeneratedOnAdd();

                detail.Property(detail => detail.GuestsNumber)
                    .IsRequired();

                detail.Property(detail => detail.ReservationId)
                    .IsRequired();

                detail.Property(detail => detail.RoomId)
                    .IsRequired();

                detail.HasOne(detail => detail.Reservation)
                    .WithMany(reservation => reservation.DetailReservations)
                    .HasForeignKey(detail => detail.ReservationId);

                detail.HasOne(detail => detail.Room)
                    .WithMany(room => room.DetailReservations)
                    .HasForeignKey(detail => detail.RoomId);
            });
        }
    }
}

