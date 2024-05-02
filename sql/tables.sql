CREATE SCHEMA `mmo` ;

CREATE TABLE `mmo`.`characters` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `account_id` INT UNSIGNED NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `zone_id` INT UNSIGNED NULL,
  `positionX` SMALLINT UNSIGNED NULL,
  `positionY` SMALLINT UNSIGNED NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE
  );
  
  CREATE TABLE mmo.accounts (
  id INT UNSIGNED NOT NULL AUTO_INCREMENT,
  username VARCHAR(45) NOT NULL,
  password BINARY(36) NOT NULL,
  PRIMARY KEY (id),
  UNIQUE INDEX id_UNIQUE (id ASC) VISIBLE,
  UNIQUE INDEX username_UNIQUE (username ASC) VISIBLE
  );
  
  ALTER TABLE `mmo`.`characters` 
ADD COLUMN `slot` TINYINT(1) UNSIGNED NOT NULL AFTER `account_id`,
ADD UNIQUE `slot_assignment`(`account_id`, `slot`);

ALTER TABLE `mmo`.`characters` 
ADD UNIQUE INDEX `slot_assignment_UNIQUE` (`account_id` ASC, `slot` ASC) VISIBLE;

ALTER TABLE `mmo`.`characters` 
ADD CONSTRAINT `account_id`
  FOREIGN KEY (`account_id`)
  REFERENCES `mmo`.`accounts` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;
  
ALTER TABLE `mmo`.`characters` 
ADD COLUMN `class` TINYINT(1) UNSIGNED NOT NULL AFTER `positionY`;

ALTER TABLE `mmo`.`characters` 
ADD COLUMN `level` SMALLINT(2) UNSIGNED NOT NULL DEFAULT 1 AFTER `class`;

ALTER TABLE `mmo`.`characters` 
CHANGE COLUMN `zone_id` `zone_id` INT UNSIGNED NOT NULL DEFAULT 0 ,
CHANGE COLUMN `positionX` `positionX` SMALLINT UNSIGNED NOT NULL DEFAULT 0 ,
CHANGE COLUMN `positionY` `positionY` SMALLINT UNSIGNED NOT NULL DEFAULT 0 ;