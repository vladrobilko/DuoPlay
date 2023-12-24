using Microsoft.EntityFrameworkCore;

namespace DuoPlay.DataManagement
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<PlayareaDto> Playareas { get; set; }

        public virtual DbSet<PlayerDto> Players { get; set; }

        public virtual DbSet<SeabattleGameDto> SeabattleGames { get; set; }

        public virtual DbSet<TicTakToeGameDto> TictaktoeGames { get; set; }

        public virtual DbSet<SessionDto> Sessions { get; set; }

        public virtual DbSet<ShootDto> Shoots { get; set; }

        public virtual DbSet<ShipDto> Ships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayareaDto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("playareas_pkey");

                entity.ToTable("playareas");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.IdPlayer)
                    .ValueGeneratedNever()
                    .HasColumnName("id_player");

                entity.Property(e => e.ConfirmedPlayarea)
                    .HasPrecision(0)
                    .HasColumnName("confirmed_playarea");

                entity.Property(e => e.Playarea1).HasColumnName("playarea");
            });

            modelBuilder.Entity<PlayerDto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("players_pkey");

                entity.ToTable("players");

                entity.HasIndex(e => e.Name, "players_name_unique").IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<SeabattleGameDto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("seabattle_games_pkey");

                entity.ToTable("seabattle_games");

                entity.HasIndex(e => e.IdSession, "seabattle_games_id_session_unique").IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");
                entity.Property(e => e.EndGame)
                    .HasPrecision(0)
                    .HasColumnName("end_game");
                entity.Property(e => e.GameMessage).HasColumnName("game_message");

                entity.Property(e => e.IdPlayerTurn).HasColumnName("id_player_turn");
                entity.Property(e => e.IdPlayerWin).HasColumnName("id_player_win");
                entity.Property(e => e.IdSession).HasColumnName("id_session");
                entity.Property(e => e.StartGame)
                    .HasPrecision(0)
                    .HasColumnName("start_game");

                entity.HasOne(d => d.IdPlayerTurnNavigation).WithMany(p => p.SeabattleGameIdPlayerTurnNavigations)
                    .HasForeignKey(d => d.IdPlayerTurn)
                    .HasConstraintName("seabattle_games_id_player_turn_foreign");

                entity.HasOne(d => d.IdPlayerWinNavigation).WithMany(p => p.SeabattleGameIdPlayerWinNavigations)
                    .HasForeignKey(d => d.IdPlayerWin)
                    .HasConstraintName("seabattle_games_id_player_win_foreign");

                entity.HasOne(d => d.IdSessionNavigation).WithOne(p => p.SeabattleGame)
                    .HasForeignKey<SeabattleGameDto>(d => d.IdSession)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("seabattle_games_id_session_foreign");
            });

            modelBuilder.Entity<TicTakToeGameDto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("tictaktoe_games_pkey");

                entity.ToTable("tictaktoe_games");

                entity.HasIndex(e => e.IdSession, "tictaktoe_games_id_session_unique").IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.EndGame)
                    .HasPrecision(0)
                    .HasColumnName("end_game");

                entity.Property(e => e.GameMessage).HasColumnName("game_message");

                entity.Property(e => e.IdPlayerTurn).HasColumnName("id_player_turn");

                entity.Property(e => e.IdPlayerWin).HasColumnName("id_player_win");

                entity.Property(e => e.IdSession).HasColumnName("id_session");

                entity.Property(e => e.StartGame)
                    .HasPrecision(0)
                    .HasColumnName("start_game");

                entity.HasOne(d => d.IdPlayerTurnNavigation).WithMany(p => p.TicTakToeGameIdPlayerTurnNavigations)
                    .HasForeignKey(d => d.IdPlayerTurn)
                    .HasConstraintName("tictaktoe_games_id_player_turn_foreign");

                entity.HasOne(d => d.IdPlayerWinNavigation).WithMany(p => p.TicTakToeGameIdPlayerWinNavigations)
                    .HasForeignKey(d => d.IdPlayerWin)
                    .HasConstraintName("tictaktoe_games_id_player_win_foreign");

                entity.HasOne(d => d.IdSessionNavigation).WithOne(p => p.TicTakToeGame)
                    .HasForeignKey<TicTakToeGameDto>(d => d.IdSession)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tictaktoe_games_id_session_foreign");

                entity.Property(e => e.BoardState).HasColumnName("board_state");
            });

            modelBuilder.Entity<SessionDto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("sessions_pkey");

                entity.ToTable("sessions");

                entity.HasIndex(e => e.Name, "sessions_name_unique").IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");
                entity.Property(e => e.EndSession)
                    .HasPrecision(0)
                    .HasColumnName("end_session");
                entity.Property(e => e.IdPlayerHost).HasColumnName("id_player_host");
                entity.Property(e => e.IdPlayerJoin).HasColumnName("id_player_join");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
                entity.Property(e => e.StartSession)
                    .HasPrecision(0)
                    .HasColumnName("start_session");

                entity.HasOne(d => d.IdPlayerHostNavigation).WithMany(p => p.SessionIdPlayerHostNavigations)
                    .HasForeignKey(d => d.IdPlayerHost)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sessions_id_player_host_foreign");

                entity.HasOne(d => d.IdPlayerJoinNavigation).WithMany(p => p.SessionIdPlayerJoinNavigations)
                    .HasForeignKey(d => d.IdPlayerJoin)
                    .HasConstraintName("sessions_id_player_join_foreign");
            });

            modelBuilder.Entity<ShootDto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("shoots_pkey");

                entity.ToTable("shoots");

                entity.HasIndex(e => e.IdSeabattleGame, "shoots_id_seabattle_game_unique").IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");
                entity.Property(e => e.IdPlayerShoot).HasColumnName("id_player_shoot");
                entity.Property(e => e.IdSeabattleGame).HasColumnName("id_seabattle_game");
                entity.Property(e => e.ShootCoordinateX).HasColumnName("shoot_coordinate_X");
                entity.Property(e => e.ShootCoordinateY).HasColumnName("shoot_coordinate_Y");
                entity.Property(e => e.TimeShoot)
                    .HasPrecision(0)
                    .HasColumnName("time_shoot");

                entity.HasOne(d => d.IdPlayerShootNavigation).WithMany(p => p.Shoots)
                    .HasForeignKey(d => d.IdPlayerShoot)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("shoots_id_player_shoot_foreign");

                entity.HasOne(d => d.IdSeabattleGameNavigation).WithOne(p => p.Shoot)
                    .HasForeignKey<ShootDto>(d => d.IdSeabattleGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("shoots_id_seabattle_game_foreign");
            });

            modelBuilder.Entity<ShipDto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("ships_pkey");

                entity.ToTable("ships");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                     .HasColumnName("id");

                entity.Property(e => e.IdPlayarea).HasColumnName("id_playarea");
                entity.Property(e => e.Length).HasColumnName("length");
                entity.Property(e => e.DecksJson).HasColumnName("decks_json");

                entity.HasOne(d => d.IdPlayareaNavigation).WithMany(p => p.Ships)
                      .HasForeignKey(d => d.IdPlayarea)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("ships_id_playarea");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
