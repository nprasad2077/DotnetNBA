using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetNBA.Models
{
    [Table("graphql_api_shotchartdata")]
    public class ShotChartData
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("player_name")]
        [MaxLength(255)]
        public string PlayerName { get; set; } = string.Empty;

        [Column("top")]
        public int? Top { get; set; }

        [Column("left")]
        public int? Left { get; set; }

        [Required]
        [Column("date")]
        [MaxLength(255)]
        public string Date { get; set; } = string.Empty;

        [Required]
        [Column("qtr")]
        [MaxLength(255)]
        public string Qtr { get; set; } = string.Empty;

        [Required]
        [Column("time_remaining")]
        [MaxLength(12)]
        public string TimeRemaining { get; set; } = string.Empty;

        [Column("result")]
        public bool? Result { get; set; }

        [Required]
        [Column("shot_type")]
        [MaxLength(6)]
        public string ShotType { get; set; } = string.Empty;

        [Column("distance_ft")]
        public int? DistanceFt { get; set; }

        [Column("lead")]
        public bool? Lead { get; set; }

        [Column("team_score")]
        public int? TeamScore { get; set; }

        [Column("opponent_team_score")]
        public int? OpponentTeamScore { get; set; }

        [Required]
        [Column("opponent")]
        [MaxLength(30)]
        public string Opponent { get; set; } = string.Empty;

        [Required]
        [Column("team")]
        [MaxLength(30)]
        public string Team { get; set; } = string.Empty;

        [Column("season")]
        public int? Season { get; set; }

        [Required]
        [Column("player_id")]
        [MaxLength(255)]
        public string PlayerId { get; set; } = string.Empty;
    }
}
