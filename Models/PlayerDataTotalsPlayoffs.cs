using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetNBA.Models
{
    [Table("graphql_api_playerdatatotalsplayoffs")]
    public class PlayerDataTotalsPlayoffs
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("player_name")]
        [MaxLength(255)]
        public string PlayerName { get; set; }

        [Column("position")]
        [MaxLength(30)]
        public string Position { get; set; }

        [Required]
        [Column("age")]
        public int Age { get; set; }

        [Column("games")]
        public int? Games { get; set; }

        [Column("games_started")]
        public int? GamesStarted { get; set; }

        [Column("minutes_pg")]
        public decimal? MinutesPg { get; set; }
        
        [Column("field_goals")]
        public int? FieldGoals { get; set; }
        
        [Column("field_attempts")]
        public int? FieldAttempts { get; set; }

        [Column("field_percent")]
        public decimal? FieldPercent { get; set; }

        [Column("three_fg")]
        public int? ThreeFg { get; set; }

        [Column("three_attempts")]
        public int? ThreeAttempts { get; set; }

        [Column("three_percent")]
        public decimal? ThreePercent { get; set; }

        [Column("two_fg")]
        public int? TwoFg { get; set; }

        [Column("two_attempts")]
        public int? TwoAttempts { get; set; }

        [Column("two_percent")]
        public decimal? TwoPercent { get; set; }

        [Column("effect_fg_percent")]
        public decimal? EffectFgPercent { get; set; }

        [Column("ft")]
        public int? Ft { get; set; }

        [Column("ft_attempts")]
        public int? FtAttempts { get; set; }

        [Column("ft_percent")]
        public decimal? FtPercent { get; set; }

        [Column("offensive_rb")]
        public int? OffensiveRb { get; set; }

        [Column("defensive_rb")]
        public int? DefensiveRb { get; set; }

        [Column("total_rb")]
        public int? TotalRb { get; set; }

        [Column("assists")]
        public int? Assists { get; set; }

        [Column("steals")]
        public int? Steals { get; set; }

        [Column("blocks")]
        public int? Blocks { get; set; }

        [Column("turnovers")]
        public int? Turnovers { get; set; }

        [Column("personal_fouls")]
        public int? PersonalFouls { get; set; }

        [Column("points")]
        public int? Points { get; set; }

        [MaxLength(30)]
        [Column("team")]
        public string Team { get; set; }

        [Column("season")]
        public int? Season { get; set; }

        [MaxLength(255)]
        [Column("player_id")]
        public string PlayerId { get; set; }
    }
}
