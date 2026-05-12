CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

DROP TABLE IF EXISTS vehicles, work_registrations, servicing CASCADE;

CREATE TABLE vehicles (
                      id SERIAL PRIMARY KEY,
                      vin VARCHAR(17) UNIQUE NOT NULL,
                      registration_number VARCHAR(20) UNIQUE NOT NULL,
                      available_from DATE,
                      available_to DATE,
                      is_owned_by_company BOOLEAN NOT NULL,
                      vehicle_type VARCHAR(50) NOT NULL
);

CREATE TABLE work_registrations (
                                    id SERIAL PRIMARY KEY,
                                    clerk_username VARCHAR(255) NOT NULL,
                                    vehicle_id INTEGER REFERENCES vehicles(id) ON DELETE CASCADE NOT NULL,
                                    work_date TIMESTAMP WITH TIME ZONE NOT NULL,
                                    duration_hours NUMERIC(5, 2) NOT NULL,
                                    description TEXT NOT NULL,
                                    cost_per_hour NUMERIC(10, 2) NOT NULL
);

CREATE TABLE servicing (
                           id SERIAL PRIMARY KEY,
                           service_number VARCHAR(255) NOT NULL,
                           vehicle_id INTEGER REFERENCES vehicles(id) ON DELETE CASCADE NOT NULL,
                           clerk_username VARCHAR(255) NOT NULL,
                           issue_description TEXT NOT NULL,
                           service_date DATE NOT NULL,
                           mechanic_name VARCHAR(255) NOT NULL,
                           cost NUMERIC(10, 2) NOT NULL
);


-- 1. Insert Vehicles
WITH inserted_vehicles AS (
    INSERT INTO "vehicles" (
                            "vin",
                            "registration_number",
                            "available_from",
                            "available_to",
                            "is_owned_by_company",
                            "vehicle_type"
        )
        SELECT
            upper(substring(replace(gen_random_uuid()::text, '-', ''), 1, 17)),
            chr(65 + (n % 26)) || chr(66 + (n % 26)) || '-' || (100 + n)::text,
            CURRENT_DATE - (n * INTERVAL '1 day'),
            CURRENT_DATE + (n * INTERVAL '10 days'),
            (n % 2 = 0),
            CASE (n % 4)
                WHEN 0 THEN 'Sedan'
                WHEN 1 THEN 'SUV'
                WHEN 2 THEN 'Truck'
                ELSE 'Van'
                END
        FROM generate_series(1, 20) AS n
        RETURNING "id"
)
-- 2. Insert Work Registrations (Usage Logs)
INSERT INTO "work_registrations" (
    "clerk_username",
    "vehicle_id",
    "work_date",
    "duration_hours",
    "description",
    "cost_per_hour"
)
SELECT
    'first',
    v.id,

    -- Trips spread across the last 4 months
    CURRENT_DATE - (random() * 120)::int * INTERVAL '1 day',

    -- Duration of the shift/trip: 1.0 to 10.0 hours
    round((random() * 9 + 1)::numeric, 1),

    -- Work-related usage descriptions
    CASE (floor(random() * 8)::int)
        WHEN 0 THEN 'Express delivery of parts to northern district'
        WHEN 1 THEN 'On-site technical consultation with client'
        WHEN 2 THEN 'Inter-city transport of project documentation'
        WHEN 3 THEN 'Routine field inspection of remote infrastructure'
        WHEN 4 THEN 'Staff shuttle for corporate conference'
        WHEN 5 THEN 'Urgent equipment pickup from regional warehouse'
        WHEN 6 THEN 'Mobile sales presentation and product demo'
        ELSE 'Surveying and mapping trip for new development area'
        END,

    -- Service rate or billable rate: 30.00 to 90.00
    round((random() * 60 + 30)::numeric, 2)

FROM inserted_vehicles v
-- 4 to 9 logs per vehicle for a nice dense data set
         CROSS JOIN LATERAL (SELECT generate_series(1, floor(random() * 6 + 4)::int)) AS s;