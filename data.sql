-- Criar o banco de dados e selecioná-lo
CREATE DATABASE IF NOT EXISTS receitas;
USE receitas;

SELECT * FROM Users u ;
SELECT * FROM Recipes r ;
SELECT * FROM Ingredients i ;
SELECT * FROM Instructions i ;
SELECT * FROM DishType dt ;

SELECT * from Ingredients i join Recipes r on r.Id = i.RecipeId WHERE RecipeId = 'd443ead2-31c9-11ef-869d-0242ac110002';

SELECT DISTINCT i.*, r.Title AS RecipeTitle, instr.Step, instr.Text AS InstructionText
FROM Ingredients i
JOIN Recipes r ON r.Id = i.RecipeId
JOIN Instructions instr ON instr.RecipeId = r.Id
WHERE r.Id = 'd4442836-31c9-11ef-869d-0242ac110003';


SELECT i.Id AS IngredientId, i.CreatedOn AS IngredientCreatedOn, i.Active AS IngredientActive, i.Item AS IngredientItem,
       r.Id AS RecipeId, r.CreatedOn AS RecipeCreatedOn, r.Active AS RecipeActive, r.Title AS RecipeTitle, r.CookingTime AS RecipeCookingTime, r.Difficulty AS RecipeDifficulty,
       instr.Step AS InstructionStep, instr.Text AS InstructionText
FROM Ingredients i
JOIN Recipes r ON r.Id = i.RecipeId
JOIN (
    SELECT instr.RecipeId, instr.Step, instr.Text
    FROM Instructions instr
) instr ON instr.RecipeId = r.Id
WHERE r.Id = 'd4442836-31c9-11ef-869d-0242ac110003';

SELECT i.Id AS IngredientId, i.CreatedOn AS IngredientCreatedOn, i.Active AS IngredientActive, i.Item AS IngredientItem,
       r.Id AS RecipeId, r.CreatedOn AS RecipeCreatedOn, r.Active AS RecipeActive, r.Title AS RecipeTitle, r.CookingTime AS RecipeCookingTime, r.Difficulty AS RecipeDifficulty,
       instr.Step AS InstructionStep, instr.Text AS InstructionText
FROM Ingredients i
JOIN Recipes r ON r.Id = i.RecipeId
JOIN (
    SELECT DISTINCT instr.RecipeId, instr.Step, instr.Text
    FROM Instructions instr
) instr ON instr.RecipeId = r.Id
WHERE r.Id = 'd4442836-31c9-11ef-869d-0242ac110003';





-- Inserir dados na tabela Users (se necessário)

INSERT INTO `Users` (`Id`, `CreatedOn`, `Active`, `Name`, `Email`, `Password`)
VALUES 
    ('c21f47a6-359e-4ae5-bd6c-8d0e6798a129', '2024-06-24 10:00:00', 1, 'João da Silva', 'joao@example.com', 'hashed_password'),
    ('e5e903d7-8a20-46e7-bd12-2cf45a1ae8f3', '2024-06-25 12:30:00', 1, 'Maria Souza', 'maria@example.com', 'hashed_password');


-- Inserir dados na tabela Recipes
INSERT INTO Recipes (Id, CreatedOn, Active, Title, CookingTime, Difficulty, UserId)
VALUES 
('d443ead2-31c9-11ef-869d-0242ac110002', NOW(), TRUE, 'Spaghetti Carbonara', 30, 2, 'c21f47a6-359e-4ae5-bd6c-8d0e6798a129'),
('d4442836-31c9-11ef-869d-0242ac110002', NOW(), TRUE, 'Chicken Curry', 45, 3, 'c21f47a6-359e-4ae5-bd6c-8d0e6798a129'),
('d4442836-31c9-11ef-869d-0242ac110003', NOW(), TRUE, 'Beef Stew', 120, 4, 'c21f47a6-359e-4ae5-bd6c-8d0e6798a129');

-- Inserir dados na tabela Ingredients
INSERT INTO Ingredients (Id, CreatedOn, Active, Item, RecipeId)
VALUES 
(UUID(), NOW(), TRUE, 'Spaghetti', 'd443ead2-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 'Eggs', 'd443ead2-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 'Pancetta', 'd443ead2-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 'Chicken', 'd4442836-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 'Curry Powder', 'd4442836-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 'Coconut Milk', 'd4442836-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 'Beef', 'd4442836-31c9-11ef-869d-0242ac110003'),
(UUID(), NOW(), TRUE, 'Potatoes', 'd4442836-31c9-11ef-869d-0242ac110003'),
(UUID(), NOW(), TRUE, 'Carrots', 'd4442836-31c9-11ef-869d-0242ac110003');

-- Inserir dados na tabela Instructions
INSERT INTO Instructions (Id, CreatedOn, Active, Step, Text, RecipeId)
VALUES 
(UUID(), NOW(), TRUE, 1, 'Ferva o spaghetti.', 'd443ead2-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 2, 'Frite o pancetta.', 'd443ead2-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 3, 'Misture os ovos com queijo e combine com spaghetti e pancetta.', 'd443ead2-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 1, 'Corte o frango em pedaços.', 'd4442836-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 2, 'Frite o frango com curry em pó.', 'd4442836-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 3, 'Adicione leite de coco e cozinhe em fogo baixo.', 'd4442836-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 1, 'Corte a carne em cubos.', 'd4442836-31c9-11ef-869d-0242ac110003'),
(UUID(), NOW(), TRUE, 2, 'Doure a carne na panela.', 'd4442836-31c9-11ef-869d-0242ac110003'),
(UUID(), NOW(), TRUE, 3, 'Adicione vegetais e cozinhe até ficar macio.', 'd4442836-31c9-11ef-869d-0242ac110003');

-- Inserir dados na tabela DishType
INSERT INTO DishTypes (Id, CreatedOn, Active, Type, RecipeId)
VALUES 
(UUID(), NOW(), TRUE, 1, 'd443ead2-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 1, 'd4442836-31c9-11ef-869d-0242ac110002'),
(UUID(), NOW(), TRUE, 1, 'd4442836-31c9-11ef-869d-0242ac110003');
