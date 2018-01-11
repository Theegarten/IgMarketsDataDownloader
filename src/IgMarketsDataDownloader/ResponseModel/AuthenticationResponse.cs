﻿using System.Collections.Generic;
using IgMarketsDataDownloader.Entity.Authorization;

namespace IgMarketsDataDownloader.ResponseModel
{
    public class AuthenticationResponse
    {
        ///<Summary>
        ///Account type
        ///</Summary>
        public string AccountType { get; set; }

        ///<Summary>
        ///Active account summary
        ///</Summary>
        public AccountInfo AccountInfo { get; set; }

        ///<Summary>
        ///Account currency
        ///</Summary>
        public string CurrencyIsoCode { get; set; }

        ///<Summary>
        ///Account currency symbol
        ///</Summary>
        public string CurrencySymbol { get; set; }

        ///<Summary>
        ///Active account identifier
        ///</Summary>
        public string CurrentAccountId { get; set; }

        ///<Summary>
        ///Lightstreamer endpoint for subscribing to account and price updates
        ///</Summary>
        public string LightstreamerEndpoint { get; set; }

        ///<Summary>
        ///Client account summaries
        ///</Summary>
        public List<AccountDetails> Accounts { get; set; }

        ///<Summary>
        ///Client identifier
        ///</Summary>
        public string ClientId { get; set; }

        ///<Summary>
        ///Client account timezone offset relative to UTC, expressed in hours
        ///</Summary>
        public int TimezoneOffset { get; set; }

        ///<Summary>
        ///Whether the Client has active demo accounts.
        ///</Summary>
        public bool HasActiveDemoAccounts { get; set; }

        ///<Summary>
        ///Whether the Client has active live accounts.
        ///</Summary>
        public bool HasActiveLiveAccounts { get; set; }

        ///<Summary>
        ///Whether the account is allowed to set trailing stops on his trades
        ///</Summary>
        public bool TrailingStopsEnabled { get; set; }

        ///<Summary>
        ///If specified, indicates that the authentication process requires the client to switch to a different URL in order to complete the login.
        ///If null, no rerouting has to take place and the authentication process is complete.
        ///This is expected for any DEMO clients, where the authentication process is initiated against the production servers (i.e. https://api.ig.com/gateway/deal )
        ///whereas all subsequent requests have to be issued against the DEMO servers (i.e. https://demo-api.ig.com/gateway/deal )
        ///Please also note that when rerouting to DEMO it is also required to invoke to "silent login" endpoint in DEMO with the CST token
        ///obtained by the preceding LIVE authentication endpoint invocation.
        ///Please consult the http://labs.ig.com site for more details about the login rerouting details.
        ///</Summary>
        public string ReroutingEnvironment { get; set; }

        ///<Summary>
        ///</Summary>
        public string IgCompany { get; set; }

        ///<Summary>
        ///</Summary>
        public string KycFormUrl { get; set; }
    }
}