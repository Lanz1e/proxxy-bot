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


        public SimpleUserHandler(Bot bot, SteamID sid) : base(bot, sid)
        {

            //INICIAR IA DA PROXXY
            AimlBot = new AIMLbot.Bot();
            myUser = new User("steamFriend", AimlBot);
            AimlBot.loadSettings();
            AimlBot.isAcceptingUserInput = false;
            AimlBot.loadAIMLFromFiles();
            AimlBot.isAcceptingUserInput = true;
        }

        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static string removerAcentos(string texto)
        {
            string comAcentos = "ƒ≈¡¬¿√‰·‚‡„… À»ÈÍÎËÕŒœÃÌÓÔÏ÷”‘“’ˆÛÙÚı‹⁄€¸˙˚˘«Á";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            return texto;
        }

        public override bool OnGroupAdd()
        {
            return false;
        }

        public override bool OnFriendAdd()
        {
            return true;
        }

        public override void OnLoginCompleted()
        {
            //Bot.SteamFriends.SendChatMessage(76561198070151287, EChatEntryType.ChatMsg,
                //"PROXXY CHAMANDO LANZ1E.. PROXXY CHAMANDO LANZ1E, TESTANDO.. TESTANDO.. 1,2,3!");
        }

        public override void OnChatRoomMessage(SteamID chatID, SteamID sender, string message)
        {
            Log.Info(Bot.SteamFriends.GetFriendPersonaName(sender) + ": " + message);
            base.OnChatRoomMessage(chatID, sender, message);
        }

        public override void OnFriendRemove() { }

        public override void OnMessage(string message, EChatEntryType type)
        {
            message = message.ToLower();
            message = removerAcentos(message);
            //log de msgs
            Console.WriteLine(DateTime.Now.ToShortTimeString().ToString() + "-" + Bot.SteamFriends.GetFriendPersonaName(OtherSID) + ": " + message);

            if (IsNumeric(message))//brincadeira com numeros
            {
                Random rnd = new Random();
                Double a = Double.Parse(message);
                Double b = 0.0;
                Double res = 0.0;
                string mResp = "";

                int op = rnd.Next(1, 6);
                switch (op)
                {
                    case 1://adi
                        b = rnd.Next(1, 9999);
                        res = a + b;
                        mResp = a + " + " + b + " È " + res.ToString();
                        SendChatMessage(mResp);
                        Console.WriteLine(DateTime.Now.ToShortTimeString().ToString() + "-" + Bot.DisplayName + ": " + mResp);
                        break;
                    case 2://sub
                        b = rnd.Next(1, 9999);
                        res = a - b;
                        mResp = a + " - " + b + " È " + res.ToString();
                        SendChatMessage(mResp);
                        Console.WriteLine(DateTime.Now.ToShortTimeString().ToString() + "-" + Bot.DisplayName + ": " + mResp);
                        break;
                    case 3://mul
                        b = rnd.Next(1, 9999);
                        res = a * b;
                        mResp = a + " * " + b + " È " + res.ToString();
                        SendChatMessage(mResp);
                        Console.WriteLine(DateTime.Now.ToShortTimeString().ToString() + "-" + Bot.DisplayName + ": " + mResp);
                        break;
                    case 4://div
                        b = rnd.Next(1, 9999);
                        res = a / b;
                        mResp = a + " / " + b + " È " + res.ToString();
                        SendChatMessage(mResp);
                        Console.WriteLine(DateTime.Now.ToShortTimeString().ToString() + "-" + Bot.DisplayName + ": " + mResp);
                        break;
                    case 5://bin
                        int x = (int)Math.Round(a, MidpointRounding.AwayFromZero);
                        mResp = a + " em bin·rio È " + Convert.ToString(x, 2);
                        SendChatMessage(mResp);
                        Console.WriteLine(DateTime.Now.ToShortTimeString().ToString() + "-" + Bot.DisplayName + ": " + mResp);
                        break;
                }
            }
            else if (message.Length == 1 && !(Regex.IsMatch(message, @"^\d+$")))//letra
            {
                char l = Char.Parse(message);
                if (l == 'z')
                    l = 'a';
                else
                    l++;
                SendChatMessage(l.ToString());
                Console.WriteLine(DateTime.Now.ToShortTimeString().ToString() + "-" + Bot.DisplayName + ": " + l);
            }
            else
            {//resp com ia
                Request r = new Request(message, myUser, AimlBot);
                Result res = AimlBot.Chat(r);
                SendChatMessage(res.Output);
                Console.WriteLine(DateTime.Now.ToShortTimeString().ToString() + "-" + Bot.DisplayName + ": " + res.Output);
            }
        }

        public override bool OnTradeRequest()
        {
            if (IsAdmin)
            {
                SendChatMessage("uhuu!");
                //Bot.SteamFriends.SendChatMessage(OtherSID,EChatEntryType.ChatMsg, "uhuu!");
                return true;
            }
            SendChatMessage("desculpa, mas por enquanto sÛ posso aceitar trocas do meu mestre");
            /*Bot.SteamFriends.SendChatMessage(OtherSID, EChatEntryType.ChatMsg, 
                "desculpa, mas por enquanto sÛ posso aceitar trocas do meu mestre");*/
            return false;
        }

        public override void OnTradeError(string error)
        {
            SendChatMessage("oh, ocorreu um erro: {0}.", error);
            Log.Warn(error);
        }

        public override void OnTradeTimeout()
        {
            SendChatMessage("desculpa, mas vocÍ estava AFK e a troca foi cancelada.");
            Log.Info("User was kicked because he was AFK.");
        }

        public override void OnTradeInit()
        {
            SendTradeMessage("o que vocÍ quer trocar ?");
        }

        public override void OnTradeAddItem(Schema.Item schemaItem, Inventory.Item inventoryItem) { }

        public override void OnTradeRemoveItem(Schema.Item schemaItem, Inventory.Item inventoryItem) { }

        public override void OnTradeMessage(string message) { }

        public override void OnTradeReady(bool ready)
        {
            if (!ready)
            {
                Trade.SetReady(false);
            }
            else
            {
                if (Validate())
                {
                    Trade.SetReady(true);
                }
                SendTradeMessage("Scrap: {0}", AmountAdded.ScrapTotal);
            }
        }

        public override void OnTradeAwaitingConfirmation(long tradeOfferID)
        {
            Log.Warn("Trade ended awaiting confirmation");
            SendChatMessage("nice, confirmar aÌ pra gente terminar a troca.");
        }

        public override void OnTradeOfferUpdated(TradeOffer offer)
        {
            switch (offer.OfferState)
            {
                case TradeOfferState.TradeOfferStateAccepted:
                    //Log.Info($"Trade offer {offer.TradeOfferId} has been completed!");
                    SendChatMessage("troca completa, valeuuu <3!");
                    break;
                case TradeOfferState.TradeOfferStateActive:
                case TradeOfferState.TradeOfferStateNeedsConfirmation:
                case TradeOfferState.TradeOfferStateInEscrow:
                    //Trade is still active but incomplete
                    break;
                case TradeOfferState.TradeOfferStateCountered:
                    //.Info($"Trade offer {offer.TradeOfferId} was countered");
                    break;
                default:
                    //Log.Info($"Trade offer {offer.TradeOfferId} failed");
                    break;
            }
        }

        public override void OnTradeAccept()
        {
            if (Validate() || IsAdmin)
            {
                //Even if it is successful, AcceptTrade can fail on
                //trades with a lot of items so we use a try-catch
                try
                {
                    if (Trade.AcceptTrade())
                        Log.Success("Trade Accepted!");
                }
                catch
                {
                    Log.Warn("The trade might have failed, but we can't be sure.");
                }
            }
        }

        public bool Validate()
        {
            AmountAdded = TF2Value.Zero;

            List<string> errors = new List<string>();

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
                    var schemaItem = Trade.CurrentSchema.GetItem(item.Defindex);
                    errors.Add("Item " + schemaItem.Name + " is not a metal.");
                }
            }

            if (AmountAdded == TF2Value.Zero)
            {
                errors.Add("vocÍ deve pelo menos colocar um Scrap.");
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

