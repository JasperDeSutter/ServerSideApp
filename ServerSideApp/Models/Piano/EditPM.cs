using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ServerSideApp.Repositories;

namespace ServerSideApp.Models.Piano
{
    public class EditPM : Piece
    {
        public IEnumerable<SelectListItem> GenreSelects { get; set; }
        public IEnumerable<SelectListItem> DifficulitySelects { get; set; }

        public static EditPM Create() {
            var genres = PianoGenreRepository.GetAll();
            var difficulties = PianoDifficultyRepository.GetAll();
            var genreSelects = genres.Select(genre => new SelectListItem{Text = genre.Name, Value = "" + genre.Id});
            var difficulitySelects = difficulties.Select(difficulty => new SelectListItem { Text = difficulty.ToString(), Value = "" + difficulty.Id });

            return new EditPM {
                GenreSelects = genreSelects,
                DifficulitySelects = difficulitySelects
            };
        }
    }
}