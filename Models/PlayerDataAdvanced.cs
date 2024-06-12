using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetNBA.Models
{
    [Table("graphql_api_playerdataadvanced")]
    public class PlayerDataAdvanced
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
        public string Position { get; set; } = string.Empty;

        [Required]
        [Column("age")]
        public int Age { get; set; }

        [Column("games")]
        public int? Games { get; set; }

        [Column("minutes_played")]
        public int? MinutesPlayed { get; set; }

        [Column("per")]
        public decimal? PER { get; set; }

        [Column("ts_percent")]
        public decimal? TSPercent { get; set; }

        [Column("three_p_ar")]
        public decimal? ThreePAR { get; set; }

        [Column("ftr")]
        public decimal? FTR { get; set; }

        [Column("offensive_rb_percent")]
        public decimal? OffensiveRBPercent { get; set; }

        [Column("defensive_rb_percent")]
        public decimal? DefensiveRBPercent { get; set; }

        [Column("total_rb_percent")]
        public decimal? TotalRBPercent { get; set; }

        [Column("assist_percent")]
        public decimal? AssistPercent { get; set; }

        [Column("steal_percent")]
        public decimal? StealPercent { get; set; }

        [Column("block_percent")]
        public decimal? BlockPercent { get; set; }

        [Column("turnover_percent")]
        public decimal? TurnoverPercent { get; set; }

        [Column("usage_percent")]
        public decimal? UsagePercent { get; set; }

        [Column("offensive_ws")]
        public decimal? OffensiveWS { get; set; }

        [Column("defensive_ws")]
        public decimal? DefensiveWS { get; set; }

        [Column("win_shares")]
        public decimal? WinShares { get; set; }

        [Column("win_shares_per")]
        public decimal? WinSharesPer { get; set; }

        [Column("offensive_box")]
        public decimal? OffensiveBox { get; set; }

        [Column("defensive_box")]
        public decimal? DefensiveBox { get; set; }

        [Column("box")]
        public decimal? Box { get; set; }

        [Column("vorp")]
        public decimal? VORP { get; set; }

        [Column("team")]
        [MaxLength(30)]
        public string Team { get; set; } = string.Empty;

        [Column("season")]
        public int? Season { get; set; }

        [Column("player_id")]
        [MaxLength(255)]
        public string PlayerId { get; set; }
    }
}
