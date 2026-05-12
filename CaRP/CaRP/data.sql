INSERT INTO "vehicles" (
    "vin",
    "registration_number",
    "available_from",
    "available_to",
    "is_owned_by_company",
    "vehicle_type"
)
SELECT
    -- Generates a random 17-char string using UUID
    upper(substring(replace(gen_random_uuid()::text, '-', ''), 1, 17)) AS "Vin",

    -- Generates a dummy Registration Number (e.g., AB-100)
    chr(65 + (n % 26)) || chr(66 + (n % 26)) || '-' || (100 + n)::text AS "RegistrationNumber",

    -- Dates: Available from today, or null for every 3rd entry
    CASE WHEN n % 3 = 0 THEN NULL ELSE CURRENT_DATE END AS "AvailableFrom",

    -- Dates: Available until 1 year from now, or null for every 5th entry
    CASE WHEN n % 5 = 0 THEN NULL ELSE CURRENT_DATE + INTERVAL '1 year' END AS "AvailableTo",

    -- Toggle ownership (True/False)
    (n % 2 = 0) AS "IsOwnedByCompany",

    -- Cycle through types
    CASE (n % 4)
        WHEN 0 THEN 'Sedan'
        WHEN 1 THEN 'SUV'
        WHEN 2 THEN 'Truck'
        ELSE 'Van'
        END AS "VehicleType"
FROM generate_series(1, 20) AS n;