using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NTEDoc.DataRepository
{
    public class Partner : IEntity
    {
        public int Id { get; set; }
        public int IDFirme { get; set; }
        [Display(Name = "Šifra partnera:")]
        public string Konto { get; set; }
        [Display(Name = "Pun naziv:")]
        public string naziv { get; set; }
        public string Mesto { get; set; }
        public string Adresa { get; set; }
        [Display(Name = "PIB partnera:")]
        [MinLength(9, ErrorMessage = "PIB mora imati tačno 9 cifara!")]
        [MaxLength(9, ErrorMessage = "PIB mora imati tačno 9 cifara!")]
        public string PIB { get; set; }
        [Display(Name = "Matični broj")]
        [MinLength(8, ErrorMessage = "Matični broj mora imati minimum 8 cifara")]
        [MaxLength(13, ErrorMessage = "Matični broj ne može imati više od 13 cifara!")]
        public string Maticni { get; set; }
        [Display(Name = "Broj računa:")]
        public string ziro { get; set; }
        [Display(Name = "Office Tel:")]
        public string Telefon { get; set; }

        public string ptt { get; set; }
        public string Opstina { get; set; }


        public ICollection<Document> Document { get; set; }
    }
}
