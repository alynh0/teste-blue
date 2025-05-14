namespace AgendaApi.Application.Contacts.DTOs
{
    public record ContactDto(
        Guid Id,
        string Name,
        string Email,
        string Phone
    );
}
