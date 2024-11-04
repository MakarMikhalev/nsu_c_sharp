DO $$
    BEGIN
        IF NOT EXISTS (SELECT FROM pg_database WHERE datname = 'hackathon') THEN
            CREATE DATABASE hackathon;
        END IF;
    END $$;
