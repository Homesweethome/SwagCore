using System;
using ApiAiSDK;

namespace SwagCore.Ai
{
    public class Dialogflow : IDialogflow
    {
        private ApiAi _ai;

        public void Connect(string clientAccessToken)
        {
            _ai = new ApiAi(new AIConfiguration(clientAccessToken, SupportedLanguage.Russian));
        }

        public string SendMessage(string message)
        {
            var response =_ai.TextRequest(message);
            return response.Result.Fulfillment.Speech;            
        }
    }
}
