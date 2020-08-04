CREATE DATABASE myproject;

/* Your DB username and sassword
username=myproject
password=myproject
*/


/** COPY THE FOLLOWING TABLES **/

/*
 * Users
 */

CREATE TABLE users
(
    id serial PRIMARY KEY,
    login_name varchar NOT NULL,
    password varchar NOT NULL,
    full_name varchar,
    active boolean NOT NULL
);

CREATE INDEX ON users (login_name, password);

CREATE TABLE user_role
(
    user_id int NOT NULL REFERENCES users,
    role varchar NOT NULL,
    PRIMARY KEY (user_id, role)
);

/* Insert Admin User */
INSERT INTO users ("id", "login_name", "password", "full_name", "active") VALUES ('1', 'admin', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'Admin', true);
INSERT INTO user_role ("user_id", "role") VALUES ('1', 'ADMIN');

INSERT INTO users ("id", "login_name", "password", "full_name", "active") VALUES ('2', 'john', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'John Smith', true);
INSERT INTO user_role ("user_id", "role") VALUES ('2', 'STAFF');


/*
 * Projects
 */
CREATE TABLE projects
(
  id serial PRIMARY KEY,
  project_name varchar,
  project_description varchar,
  project_deadline timestamp,
  assign_to int NOT NULL REFERENCES users,
  time_updated timestamp
);
CREATE INDEX ON projects (project_name);

