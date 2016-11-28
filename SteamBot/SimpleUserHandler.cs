using SteamKit2;
using System.Collections.Generic;
using SteamTrade;
using SteamTrade.TradeOffer;
using SteamTrade.TradeWebAPI;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using AIMLbot;

namespace SteamBot
{
    public class SimpleUserHandler : UserHandler
    {
        public TF2Value AmountAdded;
        //bool desconhece = true;
        Random r = new Random();
        AIMLbot.Bot AimlBot;
        User myUser;


        public SimpleUserHandler (Bot bot, SteamID sid) : base(bot, sid) {

            //INICIAR IA DA PROXXY
            AimlBot = new AIMLbot.Bot();
            myUser = new User("steamFriend", AimlBot);
            AimlBot.loadSettings();
            AimlBot.isAcceptingUserInput = false;
            AimlBot.loadAIMLFromFiles();
            AimlBot.isAcceptingUserInput = true;
        }

        public static string removerAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            return texto;
        }

        public override bool OnGroupAdd()
        {
            return false;
        }

        public override bool OnFriendAdd () 
        {
            return true;
        }

        public override void OnLoginCompleted()
        {
            Bot.SteamFriends.SendChatMessage(76561198070151287, EChatEntryType.ChatMsg,
                "PROXXY CHAMANDO BULLET.. PROXXY CHAMANDO BULLET, TESTANDO.. TESTANDO.. 1,2,3!");
        }

        public override void OnChatRoomMessage(SteamID chatID, SteamID sender, string message)
        {
            Log.Info(Bot.SteamFriends.GetFriendPersonaName(sender) + ": " + message);
            base.OnChatRoomMessage(chatID, sender, message);
        }

        public override void OnFriendRemove () {}
        
        public override void OnMessage (string message, EChatEntryType type) 
        {
            message = message.ToLower();
            message = removerAcentos(message);
            Console.WriteLine(DateTime.Now.ToShortTimeString().ToString()+"-"+Bot.SteamFriends.GetFriendPersonaName(OtherSID)+": "+message);
            if (message.Length == 1 && !(Regex.IsMatch(message, @"^\d+$")))
            {
                char l = Char.Parse(message);
                if (l == 'z')
                    l = 'a';
                else
                    l++;
                SendChatMessage(l.ToString());
                Console.WriteLine(DateTime.Now.ToShortTimeString().ToString() + "-" + Bot.DisplayName + ": " + l);
            }
            else {
                Request r = new Request(message, myUser, AimlBot);
                Result res = AimlBot.Chat(r);
                SendChatMessage(res.Output);
                Console.WriteLine(DateTime.Now.ToShortTimeString().ToString() + "-" + Bot.DisplayName + ": " + res.Output);
            }
                
            /*message=message.ToLower();
            desconhece = true;
            if (new string[] { "caralho", "porra", "pqp", "fdp", "vai tomar no cu", "puta", "vsf", "carai", "desgraçada" }.Any(s => message.Contains(s))) {
                string[] padrao = { "você usa esse linguajar com uma menina ?", "não seja rude pls", "e a educação ?", "vou lavar seu teclado com sabão", "meça seus palavrões" };
                SendChatMessage(padrao[r.Next(0, padrao.Length)]);
                desconhece = false;
            }
            //se a pessoa envia uma letra, ela retorna a proxima letra do alfabeto
            else if (message.Length==1 && !(Regex.IsMatch(message, @"^\d+$"))) {
                char l = Char.Parse(message);
                if (l=='z')
                    l='a';
                else
                    l++;
                SendChatMessage(l.ToString());
                desconhece = false;
            }
            else {
                if (new string[] { "oi", "eai", "ola", "eae", "proxxy", "proxxybot" }.Any(s => message.Equals(s))) {
                    string[] padrao = { "oi", "olá", "opa, eai!", "eai", "eae", "oi "+ Bot.SteamFriends.GetFriendPersonaName(OtherSID)+" ^-^" };
                    if(IsAdmin)
                        SendChatMessage("MESSSSSTREEEEEEEEEEEEEEEE");
                    else
                        SendChatMessage(padrao[r.Next(0, padrao.Length)]);
                    SendChatMessage("td bem ?");
                    desconhece = false;
                }
                if (message.Contains("sim e vc") || message.Contains("s e vc")) {
                    SendChatMessage("to avonts haha");
                    desconhece = false;
                }
                if (new string[] { "haha", "kkk", "lol" }.Any(s => message.Contains(s)))
                {
                    string[] padrao = { "kkkkk","haha","lul","ASIUDGASHDASDFASDA"};
                    SendChatMessage(padrao[r.Next(0, padrao.Length)]);
                    desconhece = false;
                }
                if (message.Equals("sim") || message.Equals("s")) {
                    SendChatMessage("legal");
                    desconhece = false;
                }
                if (new string[] { "foi mal", "malz", "desculpa" }.Any(s => message.Contains(s))) {
                    SendChatMessage("rlx kk");
                    desconhece = false;
                }
                if (new string[] { "xau", "flw", "tchau" }.Any(s => message.Contains(s))) {
                    string[] padrao = { "Até logo, até mais ver, bon voyage, arrivederci, até mais, adeus, boa viagem, vá em paz, que a porta bata onde o sol não bate, hasta la vista, não volte mais aqui, escafeda-se, e saia logo daqui.", "tchau!", "até mais!", "flwwwwwwww <3", "hasta luego." };
                    SendChatMessage(padrao[r.Next(0, padrao.Length)]);
                    desconhece = false;
                }
            }
            if (desconhece){
                string[] padrao = { "hã?","o Bullet não me ensinou sobre isso ._.","o que \""+message+"\" significa?", Bot.ChatResponse };
                SendChatMessage(padrao[r.Next(0, padrao.Length)]);
            }*/

        }

        public override bool OnTradeRequest() 
        {
            if (IsAdmin)
            {
                SendChatMessage("uhuu!");
                //Bot.SteamFriends.SendChatMessage(OtherSID,EChatEntryType.ChatMsg, "uhuu!");
                return true;
            }
            SendChatMessage("desculpa, mas por enquanto só posso aceitar trocas do meu mestre");
            /*Bot.SteamFriends.SendChatMessage(OtherSID, EChatEntryType.ChatMsg, 
                "desculpa, mas por enquanto só posso aceitar trocas do meu mestre");*/
            return false;
        }
        
        public override void OnTradeError (string error) 
        {
            SendChatMessage("oh, ocorreu um erro: {0}.", error);
            Log.Warn (error);
        }
        
        public override void OnTradeTimeout () 
        {
            SendChatMessage("desculpa, mas você estava AFK e a troca foi cancelada.");
            Log.Info ("User was kicked because he was AFK.");
        }
        
        public override void OnTradeInit() 
        {
            SendTradeMessage("o que você quer trocar ?");
        }
        
        public override void OnTradeAddItem (Schema.Item schemaItem, Inventory.Item inventoryItem) {}
        
        public override void OnTradeRemoveItem (Schema.Item schemaItem, Inventory.Item inventoryItem) {}
        
        public override void OnTradeMessage (string message) {}
        
        public override void OnTradeReady (bool ready) 
        {
            if (!ready)
            {
                Trade.SetReady (false);
            }
            else
            {
                if(Validate ())
                {
                    Trade.SetReady (true);
                }
                SendTradeMessage("Scrap: {0}", AmountAdded.ScrapTotal);
            }
        }

        public override void OnTradeAwaitingConfirmation(long tradeOfferID)
        {
            Log.Warn("Trade ended awaiting confirmation");
            SendChatMessage("nice, confirmar aí pra gente terminar a troca.");
        }

        public override void OnTradeOfferUpdated(TradeOffer offer)
        {
            switch (offer.OfferState)
            {
                case TradeOfferState.TradeOfferStateAccepted:
                    Log.Info($"Trade offer {offer.TradeOfferId} has been completed!");
                    SendChatMessage("troca completa, valeuuu <3!");
                    break;
                case TradeOfferState.TradeOfferStateActive:
                case TradeOfferState.TradeOfferStateNeedsConfirmation:
                case TradeOfferState.TradeOfferStateInEscrow:
                    //Trade is still active but incomplete
                    break;
                case TradeOfferState.TradeOfferStateCountered:
                    Log.Info($"Trade offer {offer.TradeOfferId} was countered");
                    break;
                default:
                    Log.Info($"Trade offer {offer.TradeOfferId} failed");
                    break;
            }
        }

        public override void OnTradeAccept() 
        {
            if (Validate() || IsAdmin)
            {
                //Even if it is successful, AcceptTrade can fail on
                //trades with a lot of items so we use a try-catch
                try {
                    if (Trade.AcceptTrade())
                        Log.Success("Trade Accepted!");
                }
                catch {
                    Log.Warn ("The trade might have failed, but we can't be sure.");
                }
            }
        }

        public bool Validate ()
        {            
            AmountAdded = TF2Value.Zero;
            
            List<string> errors = new List<string> ();
            
            foreach (TradeUserAssets asset in Trade.OtherOfferedItems)
            {
                var item = Trade.OtherInventory.GetItem(asset.assetid);
                if (item.Defindex == 5000)
                    AmountAdded += TF2Value.Scrap;
                else if (item.Defindex == 5001)
                    AmountAdded += TF2Value.Reclaimed;
                else if (item.Defindex == 5002)
                    AmountAdded += TF2Value.Refined;
                else
                {
                    var schemaItem = Trade.CurrentSchema.GetItem (item.Defindex);
                    errors.Add ("Item " + schemaItem.Name + " is not a metal.");
                }
            }
            
            if (AmountAdded == TF2Value.Zero)
            {
                errors.Add ("você deve pelo menos colocar um Scrap.");
            }
            
            // send the errors
            if (errors.Count != 0)
                SendTradeMessage("houve erros com sua troca: ");
            foreach (string error in errors)
            {
                SendTradeMessage(error);
            }
            
            return errors.Count == 0;
        }
        
    }
 
}

