using System.ComponentModel.DataAnnotations;

namespace LimeTest.Reports
{
    public class PeopleReport
    {
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        
        [Display(Name = "Пол")]
        public string Gender { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Эл. адрес")]
        public string Email { get; set; }

        [Display(Name = "Картинка")]
        public string PictureMedium { get; set; }

        [Display(Name = "Котировка")]
        public string Quote { get; set; }

        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Содержание")]
        public string Content { get; set; }

        [Display(Name = "Веб-сайт")]
        public string Url { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Расстояние")]
        public double Distance { get; set; }
    }
}