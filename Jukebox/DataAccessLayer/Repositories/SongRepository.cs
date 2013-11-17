﻿using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DataAccessLayer.Conversions;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace DataAccessLayer.Repositories
{
    public class SongRepository
    {
        private MusicContainer _context { get; set; }

        public SongRepository()
        {
            _context = new MusicContainer();
            //Starts the links to the dbsets to call and go through them
        }

        //public IQueryable<GenreModel> GetGenreList()
        //{
        //    return _context.Genres.Select(g => new GenreModel
        //    {
        //        Id = g.Id,
        //        sName = g.sName
        //    });
        //}
        public IQueryable<SongModel> GetSongList()
        {
            return _context.Songs.Select(s => new SongModel
            {
                SongID = s.Id,
                SongTitle = s.Title,
                Artist = s.Artist,
                Album = s.Album,
                Genre = s.Genre,
                FilePath = s.FilePath,
                Length = s.Length
            });
        }

        public IQueryable<SongModel> GetSongList(int loginId)
        {
            return _context.Songs.Where(s => s.Accounts.Any(u => u.LoginId == loginId))
                .Select(s => new SongModel
                {
                    SongID = s.Id,
                    SongTitle = s.Title,
                    Artist = s.Artist,
                    Album = s.Album,
                    Genre = s.Genre,
                    FilePath = s.FilePath,
                    Length = s.Length
                });
        }


        public IQueryable<AccountModel> GetAccountsList()
        {
            return _context.Accounts.Select(a => new AccountModel
                {
                    LoginId = a.LoginId,
                    Username = a.Username
                });
        }
        //Returns an account list of the room to display on the playlist
        public IQueryable<AccountModel> GetAccountsList(RoomModel room)
        {
            int roomId = GetRoomId(room);
            return _context.Accounts.Where(a => a.RoomId == roomId)
               .Select(a => new AccountModel
               {
                   LoginId = a.LoginId,
                   Username = a.Username
               });
        }

        //
        public int GetRoomId(RoomModel room)
        {
            return room.RoomId;
        }
        private Account GetAccount(int loginId)
        {
            return _context.Accounts.Where(a => a.LoginId == loginId).Single();
        }

        public void Add(SongModel model)
        {
            try
            {
                Account account;
                Song entity = ModelConversions.SongModelToEntity(model);
                if (_context.Accounts.Any(a => a.Username == model.Username))
                {
                    account = _context.Accounts.First(a => a.Username == model.Username);
                }
                else
                {
                    account = new Account() { Username = model.Username };
                }
                entity.Accounts.Add(account);
                _context.Songs.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

        }

        public void Delete(SongModel model, int loginID)
        {
            Account account = GetAccount(loginID);
            account.Songs.Remove(GetSong(model.SongID));
            _context.SaveChanges();

        }

        public Song GetSong(int songID)
        {
            return _context.Songs.Where(s => s.Id == songID).Single();
        }

      

    }
}
