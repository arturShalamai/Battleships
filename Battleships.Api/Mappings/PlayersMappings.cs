using AutoMapper;
using Battleships.BLL.Models;
using Battleships.BLL;
using Battleships.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Api.Mappings
{
    public class PlayersMappings : Profile
    {
        public PlayersMappings()
        {
            CreateMap<PlayerRegisterModel, Player>();

            CreateMap<Game, GameInfoModel>();
        }
    }
}
