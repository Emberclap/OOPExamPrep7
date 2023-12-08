using BookingApp.Models.Hotels.Contacts;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class HotelRepository : IRepository<IHotel>
    {
        private List<IHotel> hotelList;

        public HotelRepository()
        {
            this.hotelList = new List<IHotel>();
        }

        public void AddNew(IHotel model) => hotelList.Add(model);
        public IReadOnlyCollection<IHotel> All() => hotelList;
        public IHotel Select(string criteria) => hotelList.FirstOrDefault(x=>x.FullName == criteria);
        
    }
}
