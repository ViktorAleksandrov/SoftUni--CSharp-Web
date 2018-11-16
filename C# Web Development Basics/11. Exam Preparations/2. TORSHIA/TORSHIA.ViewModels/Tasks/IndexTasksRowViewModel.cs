using System.Collections.Generic;

namespace TORSHIA.ViewModels.Tasks
{
    public class IndexTasksRowViewModel
    {
        public IndexTasksRowViewModel()
        {
            this.Tasks = new List<IndexTasksViewModel>();
        }

        public List<IndexTasksViewModel> Tasks { get; set; }

        public string[] Empty => new string[5 - this.Tasks.Count];
    }
}
