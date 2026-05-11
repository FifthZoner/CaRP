namespace CaRP.Shared.Dtos;

using System.Text.Json.Serialization;
public record ClerkUserResponse(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("username")] string? Username,
    [property: JsonPropertyName("first_name")] string? FirstName,
    [property: JsonPropertyName("last_name")] string? LastName,
    [property: JsonPropertyName("image_url")] string? ImageUrl,
    [property: JsonPropertyName("email_addresses")] List<ClerkEmail> EmailAddresses
);

public record ClerkEmail(
    [property: JsonPropertyName("email_address")] string EmailAddress
);