using BookingApp.Core.Contracts;
using BookingApp.Models.Bookings;
using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Hotels;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Models.Rooms;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using BookingApp.Repositories.Contracts;
using BookingApp.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Core
{
    public class Controller : IController
    {
        private IRepository<IHotel> hotels;

        public Controller()
        {
            this.hotels = new HotelRepository();
        }

        public string AddHotel(string hotelName, int category)
        {
            
            if(hotels.All().Any(x=>x.FullName == hotelName && x.Category == category))
            {
                return string.Format(OutputMessages.HotelAlreadyRegistered, hotelName);
            }
            IHotel hotel = new Hotel(hotelName, category);
            hotels.AddNew(hotel);
            return string.Format(OutputMessages.HotelSuccessfullyRegistered, category, hotelName);
        }

        public string BookAvailableRoom(int adults, int children, int duration, int category)
        {
           
            List<IRoom> rooms = new List<IRoom>();
            foreach (var hotel1 in hotels.All().OrderBy(x=>x.FullName))
            {
                foreach (var room in hotel1.Rooms.All())
                {
                    if (room.PricePerNight > 0)
                    {
                        rooms.Add(room);
                    }
                }
            }
            int guests = adults + children;
            IRoom wantedRoom = rooms.OrderBy(x=>x.BedCapacity).FirstOrDefault(r=>r.BedCapacity >= guests);
            if (!hotels.All().Any(x=>x.Category == category))
            {
                return string.Format(OutputMessages.CategoryInvalid, category);
            }
            if (wantedRoom == null)
            {
                return string.Format(OutputMessages.RoomNotAppropriate);
            }
            IHotel hotel = hotels.All().FirstOrDefault(x => x.Category == category);
            int bookingNumber = hotel.Bookings.All().Count()+1;
            IBooking booking = new Booking(wantedRoom ,duration, adults, children, bookingNumber);
            hotel.Bookings.AddNew(booking);
            return string.Format(OutputMessages.BookingSuccessful, bookingNumber, hotel.FullName);
        }

        public string HotelReport(string hotelName)
        {
            IHotel hotel = hotels.Select(hotelName);
            if (hotel == null)
            {
                return string.Format(OutputMessages.HotelNameInvalid, hotelName);
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Hotel name: {hotel.FullName}");
            sb.AppendLine($"--{hotel.Category} star hotel");
            sb.AppendLine($"--Turnover: {hotel.Turnover:F2} $");
            sb.AppendLine($"--Bookings:");
            sb.AppendLine();
            if(hotel.Bookings.All().Count == 0)
            {
                sb.AppendLine("none");
            }
            else
            {
                foreach (var booking in hotel.Bookings.All())
                {
                    
                    sb.AppendLine($"{booking.BookingSummary()}");
                    sb.AppendLine();
                }
            }
            return sb.ToString().TrimEnd();

        }

        public string SetRoomPrices(string hotelName, string roomTypeName, double price)
        {
            IHotel hotel = hotels.Select(hotelName);
            if (hotel == null)
            {
                return string.Format(OutputMessages.HotelNameInvalid, hotelName);
            }
            if(roomTypeName != nameof(DoubleBed) && roomTypeName != nameof(Apartment) && roomTypeName != nameof(Studio))
            {
                throw new ArgumentException(ExceptionMessages.RoomTypeIncorrect);
            }
            IRoom room = hotel.Rooms.Select(roomTypeName);
            if (room == null)
            {
                return string.Format(OutputMessages.RoomTypeNotCreated);
            }
            if(room.PricePerNight != 0)
            {
                throw new InvalidOperationException(ExceptionMessages.CannotResetInitialPrice);
            }
            room.SetPrice(price);
            return string.Format(OutputMessages.PriceSetSuccessfully, roomTypeName, hotelName);
        }

        public string UploadRoomTypes(string hotelName, string roomTypeName)
        {
            IHotel hotel = hotels.Select(hotelName);
           if (hotel == null)
           {
               return string.Format(OutputMessages.HotelNameInvalid, hotelName);
           }
           if (hotel.Rooms.All().Any(x => x.GetType().Name == roomTypeName))
           {
                return string.Format(OutputMessages.RoomTypeAlreadyCreated);
           }
           if (roomTypeName != nameof(DoubleBed) && roomTypeName != nameof(Apartment) && roomTypeName != nameof(Studio)) 
           {
               throw new ArgumentException(ExceptionMessages.RoomTypeIncorrect);
           }
           IRoom room;
           if (roomTypeName == nameof(DoubleBed))
           {
               room = new DoubleBed();
           }
           else if (roomTypeName == nameof(Apartment))
           {
               room = new Apartment();
           }
           else
           {
               room = new Studio();
           }
           hotel.Rooms.AddNew(room);
           return string.Format(OutputMessages.RoomTypeAdded,roomTypeName, hotelName);
        }
    }
}
