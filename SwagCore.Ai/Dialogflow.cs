using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiAiSDK;
using ApiAiSDK.Model;
using SwagCore.Ai.Models;

namespace SwagCore.Ai
{
    public class Dialogflow : IDialogflow
    {
        private ApiAi _ai;

        public void Connect(string clientAccessToken)
        {
            _ai = new ApiAi(new AIConfiguration(clientAccessToken, SupportedLanguage.Russian));
        }

        public async Task<AiResponseModel> SendMessage(string message)
        {            
            var response = await new TaskFactory<AIResponse>().StartNew(() => _ai.TextRequest(message));
            var result = new AiResponseModel
            {
                Speech = response.Result.Fulfillment.Speech,
                Action = response.Result.Action,
                Parameters = response.Result.Parameters
            };
            return result;
        }
    }
}
