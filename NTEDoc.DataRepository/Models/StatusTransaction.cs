using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTEDoc.DataRepository.Models
{
	[Table("StatusTransactions")]
	public class StatusTransaction
	{
        public int StatusId { get; set; }

        public int NextStatusId { get; set; }

        [Column("BelongsToUserId")]
        public int? BelongsToUserTypeId { get; set; }
    }
}

