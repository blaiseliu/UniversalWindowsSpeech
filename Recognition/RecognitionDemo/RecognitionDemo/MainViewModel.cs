using System;
using System.Windows.Input;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace RecognitionDemo
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private SpeechRecognizer _recognizer;

        #region Contructor
        public MainViewModel()
        {
            InitializeRecognizer();
        }
        #endregion

        #region Methods
        private async void ActivateRecognition()
        {
            var state = _recognizer.State;
            RecognizedText = string.Empty;
            try
            {
                var recognitionResult = await _recognizer.RecognizeAsync();
                RecognizedText = recognitionResult.Confidence == SpeechRecognitionConfidence.Rejected
                    ? "Rejected: what?"
                    : recognitionResult.Text;
            }
            catch (Exception ex)
            {
                RecognizedText = ex.Message;
            }
        }
        private async void InitializeRecognizer()
        {
            var language = SpeechRecognizer.SystemSpeechLanguage;
            _recognizer = new SpeechRecognizer(language);

            #region Timeouts
            _recognizer.Timeouts.BabbleTimeout = TimeSpan.FromSeconds(3);
            _recognizer.Timeouts.InitialSilenceTimeout = TimeSpan.FromSeconds(3); 
            #endregion

            #region Grammar
            var dictationConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "dictation");
            _recognizer.Constraints.Add(dictationConstraint); 
            #endregion

            var compilationResult= await _recognizer.CompileConstraintsAsync();

            if (compilationResult.Status!=SpeechRecognitionResultStatus.Success)
            {
                RecognizedText = "Unable to compile grammar.";
            }
        }

        private void _recognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            RecognizedText = args.State.ToString();
        }

        public void Dispose()
        {
            _recognizer?.Dispose();
        }

        #endregion

        #region Properties
        private string _recognizedText;
        public string RecognizedText
        {
            get { return _recognizedText; }
            set { Set(() => RecognizedText, ref _recognizedText, value); }
        }
        #endregion

        #region Commands
        private RelayCommand _activateRecognitionCommand;

        public RelayCommand ActivateRecognitionCommand
            => _activateRecognitionCommand
            ?? (_activateRecognitionCommand = new RelayCommand(ActivateRecognition));
        #endregion

    }
}