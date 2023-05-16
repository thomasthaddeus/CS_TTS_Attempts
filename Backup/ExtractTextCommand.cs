// ExtractTextCommand.cs
using System;
using System.Windows.Input;

namespace PowerPointExtractor
{
    public class ExtractTextCommand : ICommand
    {
        private readonly ExtractorViewModel viewModel;

        public ExtractTextCommand(ExtractorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // Call ExtractSlideNotes and update viewModel.OutputFiles and viewModel.StatusMessage
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }
}
