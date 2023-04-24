using ResultViewer.Server.Models;

namespace ResultViewer.Server.Dtos
{
    public class MultipleQueryResult
    {
        public List<SuperHero> Heroes { get; set; }
        public Villain Villain { get; set; }

        public MultipleQueryResult(List<SuperHero> heroes, Villain villain)
        {
            Heroes = heroes;
            Villain = villain;
        }
    }
}
