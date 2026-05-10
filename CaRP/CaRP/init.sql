CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

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