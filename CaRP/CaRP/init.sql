/*CREATE EXTENSION IF NOT EXISTS "uuid-ossp";*/

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


-- 1. Insert Vehicles (Pojazdy)
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
            CASE (n % 2)
                WHEN 0 THEN
                    chr(87) || chr(65 + (n % 20)) || ' ' ||
                    (10000 + n * 37)::text
                ELSE
                    chr(75) || chr(82) || chr(65 + (n % 15)) || ' ' ||
                    (100 + n * 7)::text || chr(65 + (n % 5))
                END,
            CURRENT_DATE - (n * INTERVAL '1 day'),
            CURRENT_DATE + (n * INTERVAL '10 days'),
            (n % 2 = 0),
            CASE (n % 4)
                WHEN 0 THEN 'Sedan'
                WHEN 1 THEN 'SUV'
                WHEN 2 THEN 'Ciężarowy'
                ELSE 'Dostawczy'
                END
        FROM generate_series(1, 20) AS n
        RETURNING "id"
),

-- 2. Insert Work Registrations (Rejestr Pracy / Wyjazdy)
     inserted_work AS (
         INSERT INTO "work_registrations" (
                                           "clerk_username",
                                           "vehicle_id",
                                           "work_date",
                                           "duration_hours",
                                           "description",
                                           "cost_per_hour"
             )
             SELECT
                 -- Randomly assigns 1 of 10 clerk usernames
                 CASE (floor(random() * 10)::int)
                     WHEN 0 THEN 'jan.kowalski'
                     WHEN 1 THEN 'anna.nowak'
                     WHEN 2 THEN 'piotr.zielinski'
                     WHEN 3 THEN 'marta.wisniewska'
                     WHEN 4 THEN 'tomasz.wojcik'
                     WHEN 5 THEN 'karolina.kaczmarek'
                     WHEN 6 THEN 'michal.mazur'
                     WHEN 7 THEN 'agnieszka.krawczyk'
                     WHEN 8 THEN 'lukasz.zajac'
                     ELSE 'magda.krupa'
                     END,
                 v.id,
                 CURRENT_DATE
                     - (random() * 120)::int * INTERVAL '1 day' -- Losowy dzień wstecz
                     + (7 + random() * 9)::int * INTERVAL '1 hour' -- Losowa godzina startu (7:00 - 16:00)
                     + (random() * 59)::int * INTERVAL '1 minute', -- Losowa minuta (0 - 59),
                 round((random() * 9 + 1)::numeric, 1),
                 -- Expanded list of 15 unique work descriptions
                 CASE (floor(random() * 15)::int)
                     WHEN 0 THEN 'Ekspresowa dostawa części do dzielnicy północnej'
                     WHEN 1 THEN 'Konsultacja techniczna na miejscu u klienta'
                     WHEN 2 THEN 'Międzymiastowy transport dokumentacji projektowej'
                     WHEN 3 THEN 'Rutynowa inspekcja terenowa odległej infrastruktury'
                     WHEN 4 THEN 'Przewóz pracowników na konferencję korporacyjną'
                     WHEN 5 THEN 'Pilny odbiór sprzętu z magazynu regionalnego'
                     WHEN 6 THEN 'Mobilna prezentacja sprzedażowa i pokaz produktu'
                     WHEN 7 THEN 'Wyjazd pomiarowy i mapowanie terenu pod nową inwestycję'
                     WHEN 8 THEN 'Audyt bezpieczeństwa w oddziale podmiejskim'
                     WHEN 9 THEN 'Dostarczenie materiałów marketingowych na targi branżowe'
                     WHEN 10 THEN 'Wizyta serwisowa u klienta strategicznego'
                     WHEN 11 THEN 'Transport próbek laboratoryjnych do analizy'
                     WHEN 12 THEN 'Objazd kontrolny punktów dystrybucyjnych'
                     WHEN 13 THEN 'Interwencyjny wyjazd do awarii sieci u kontrahenta'
                     ELSE 'Przewóz wielkogabarytowych materiałów eksploatacyjnych'
                     END,
                 round((random() * 60 + 30)::numeric, 2)
             FROM inserted_vehicles v
                      CROSS JOIN LATERAL (SELECT generate_series(1, floor(random() * 6 + 4)::int)) AS s
             RETURNING "vehicle_id"
     )

-- 3. Insert Servicing Records (Serwis / Naprawy)
INSERT INTO "servicing" (
    "service_number",
    "vehicle_id",
    "clerk_username",
    "issue_description",
    "service_date",
    "mechanic_name",
    "cost"
)
SELECT
    'ZGL-' || upper(substring(replace(gen_random_uuid()::text, '-', ''), 1, 8)),
    v.id,
    -- Randomly assigns 1 of 10 clerk usernames for reporting the breakdown
    CASE (floor(random() * 4)::int)
        WHEN 0 THEN 'jan.kowalski'
        WHEN 1 THEN 'janusz.byk'
        WHEN 2 THEN 'zenon.maciorski'
        ELSE 'eliza.sas'
        END,
    -- Expanded list of 12 detailed breakdown descriptions
    CASE (floor(random() * 12)::int)
        WHEN 0 THEN 'Przegrzanie silnika, wymieniono pęknięty wąż chłodnicy i uzupełniono płyn.'
        WHEN 1 THEN 'Ślizganie się skrzyni biegów na 3. biegu. Wymieniono komplet sprzęgła i olej.'
        WHEN 2 THEN 'Klocki hamulcowe starte do metalu. Wymiana tarcz, zacisków i klocków z przodu.'
        WHEN 3 THEN 'Awaria alternatora powodująca rozładowywanie akumulatora. Montaż nowego alternatora OEM.'
        WHEN 4 THEN 'Stuki w zawieszeniu. Wymieniono przednie amortyzatory i łączniki stabilizatora.'
        WHEN 5 THEN 'Zapaliła się kontrolka Check Engine. Zdiagnozowano i wymieniono uszkodzoną sondę lambda.'
        WHEN 6 THEN 'Wyciek płynu wspomagania. Uszczelniono przekładnię kierowniczą i wymieniono przewody.'
        WHEN 7 THEN 'Nierówna praca silnika. Wymieniono komplet świec zapłonowych oraz cewki na 2. i 4. cylindrze.'
        WHEN 8 THEN 'Uszkodzenie opony na dziurze w jezdni. Wymiana pary opon na osi przedniej i ustawienie zbieżności.'
        WHEN 9 THEN 'Brak reakcji na rozrusznik. Wymiana zwariowanego immobilizera oraz nowego akumulatora.'
        WHEN 10 THEN 'Rozszczelnienie układu klimatyzacji. Znaleziono nieszczelność, wymieniono chłodnicę klimy (skraplacz).'
        ELSE 'Silnik przeszedł w tryb awaryjny. Zablokowany zawór EGR – czyszczenie nie pomogło, zamontowano nowy.'
        END,
    CURRENT_DATE - (random() * 180)::int * INTERVAL '1 day',
    CASE (floor(random() * 5)::int)
        WHEN 0 THEN 'Jan Kowalski (Apex Auto)'
        WHEN 1 THEN 'Mariusz Nowak (Mistrz Serwisu)'
        WHEN 2 THEN 'Piotr Wiśniewski (Pro Mechanika)'
        WHEN 3 THEN 'Krzysztof Lewandowski (Warsztat Centralny)'
        ELSE 'Tomasz Kamiński (Auto-Doktor)'
        END,
    round((random() * 1100 + 150)::numeric, 2)
FROM (SELECT DISTINCT vehicle_id AS id FROM inserted_work) v
         CROSS JOIN LATERAL (SELECT generate_series(1, floor(random() * 3 + 1)::int)) AS s;