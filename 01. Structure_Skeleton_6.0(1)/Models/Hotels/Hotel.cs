using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Models.Rooms;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using BookingApp.Repositories.Contracts;
using BookingApp.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Models.Hotels
{
    public class Hotel : IHotel
    {
        private string fullName;
        private int category;
        private IRepository<IRoom> roomRepository;
        private IRepository<IBooking> bookingRepository;

        public Hotel(string fullName, int category)
        {
            FullName = fullName;
            Category = category;
            roomRepository = new RoomRepository();
            bookingRepository = new BookingRepository();
        }

        public string FullName
        {
            get => fullName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.HotelNameNullOrEmpty);
                }
                fullName = value;
            }
            
        }

        public int Category
        {
            get => category;
            private set
            {
                if (value < 1 || value > 5)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidCategory);
                }
                category = value;
            }
        }

        public double Turnover => bookingRepository.All().Sum(x => x.Room.PricePerNight * x.ResidenceDuration);

        public IRepository<IRoom> Rooms => roomRepository;

        public IRepository<IBooking> Bookings => bookingRepository;
    }
}
