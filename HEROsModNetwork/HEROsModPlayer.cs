﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Terraria;

namespace HEROsMod.HEROsModNetwork
{
    public class HEROsModPlayer
    {
        private int _playerIndex = -1;

        public int Index
        {
            get { return _playerIndex; }
        }

		//public ServerSock ServerInstance
		//{
		//	get
		//	{
		//		return Netplay.serverSock[_playerIndex];
		//	}
		//}

		// Changed all the actives to IsActive
		// All name to Name
		// state to State
		// StatusText2  statusText2 
		//StatusMax statusMax

		public RemoteClient ServerInstance
		{
			get
			{
				return Netplay.Clients[_playerIndex];
			}
		}

		public Player GameInstance
        {
            get
            {
                return Main.player[_playerIndex];
            }
        }

        public int ID { get; set; }

        public Group Group { get; set; }
        public bool UsingHEROsMod { get; set; }
        public string Username { get; set; }

        public bool BackupHostility { get; set; }
        public int BackupTeam { get; set; }

   //     public CTF.TeamColor CTFTeam { get; set; }
        public HEROsModPlayer(int playerIndex)
        {
            Reset();
            this._playerIndex = playerIndex;
        }

        public void Reset()
        {
            this.Username = String.Empty;
            this.ID = -1;
            this.UsingHEROsMod = false;
    //        this.CTFTeam = CTF.TeamColor.None;
            Group = Network.NotLoggedInGroup;
        }
    }

    public class UserWithID
    {
        public string Username = string.Empty;
        public int ID = -1;
		public int groupID = -2;
    }
}
