﻿using Application.Booking.Ports;
using Application.Booking.DTO;
using Domain.Ports;
using Application.Booking.Responses;
using Domain.Booking.DomainExceptions;
using Application.Responses;

namespace Application.Booking
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IGuestRepository _guestRepository ;

        public BookingManager(IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IGuestRepository guestRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
        }

        //TO DO TRATAR TODAS EXCEPTIONS
        public async Task<BookingResponse> CreateBooking(BookingDTO bookingDTO)
        {
            try
            {
                var booking = BookingDTO.MapToEntity(bookingDTO);
                booking.Guest = await _guestRepository.Get(bookingDTO.GuestId);
                booking.Room = await _roomRepository.GetAggreagate(bookingDTO.RoomId);

                await booking.Save(_bookingRepository);

                bookingDTO.Id = bookingDTO.Id;

                return new BookingResponse
                {
                    Success = true,
                    Data = bookingDTO,
                };
            }
            catch (PlacedAtIsARequiredInformationException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "PlacedAt is a required information"
                };
            }
            catch (StartDateTimeIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Start is a required information"
                };
            }
            catch (EndDateTimeIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "End is a required information"
                };
            }
            catch (RomIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Room is a required information"
                };
            }
            catch (GuestIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Guest is a required information"
                };
            }
            catch (RoomCannotBeBookedException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_ROOM_CANNOT_BE_BOOKED,
                    Message = "The Selected Room is not available"
                };
            }
            catch (Exception ex)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public Task<BookingDTO> GetBooking(int bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
