
IF NOT EXISTS (SELECT * FROM Product)
BEGIN

	INSERT INTO [StockDb].[dbo].[Product] ([Id], [Name], [Description], [Price], [Category], [Quantity])
	VALUES 
	  (NEWID(), 'Laptop Dell XPS 13', 'Ultrabook com processador Intel i7 e SSD de 512GB', 8999.99, 1, 10), -- Electronics
	  (NEWID(), 'Smartphone Samsung Galaxy S23', 'Celular Android com câmera de 50MP', 5999.90, 1, 25), -- Electronics
	  (NEWID(), 'Camiseta Nike Dry Fit', 'Camiseta esportiva de tecido tecnológico', 129.90, 2, 50), -- Clothing
	  (NEWID(), 'Jaqueta de Couro Preta', 'Jaqueta de couro legítimo, estilo clássico', 799.90, 2, 15), -- Clothing
	  (NEWID(), 'Chocolate Lindt 70%', 'Chocolate amargo premium, barra de 100g', 29.90, 3, 100), -- Food
	  (NEWID(), 'Cadeira de Escritório Ergonômica', 'Cadeira ajustável com apoio lombar', 1299.90, 4, 8), -- Furniture
	  (NEWID(), 'Monitor LG Ultrawide 29"', 'Monitor IPS com resolução 2560x1080', 1599.90, 1, 12), -- Electronics
	  (NEWID(), 'Livro Clean Code', 'Livro sobre boas práticas de programação', 199.90, 5, 30), -- Books
	  (NEWID(), 'LEGO Star Wars Millennium Falcon', 'Modelo colecionável da Millennium Falcon', 1499.90, 6, 5), -- Toys
	  (NEWID(), 'Perfume Chanel No. 5', 'Fragrância feminina icônica', 999.90, 7, 20), -- Beauty
	  (NEWID(), 'Bola de Futebol Adidas', 'Bola oficial da Copa do Mundo', 149.90, 8, 40), -- Sports
	  (NEWID(), 'Óleo Sintético Mobil 1 5W30', 'Óleo sintético para motor de alto desempenho', 89.90, 9, 60), -- Automotive
	  (NEWID(), 'Kit de Suplementos Whey Protein', 'Whey Protein + BCAA + Creatina', 299.90, 10, 25); -- Health

	INSERT INTO [StockDb].[dbo].[Product] ([Id], [Name], [Description], [Price], [Category], [Quantity])
	VALUES 
	  -- Eletrônicos (1)
	  (NEWID(), 'iPhone 15 Pro', 'Smartphone Apple com chip A17 Pro', 8999.99, 1, 30),
	  (NEWID(), 'Notebook MacBook Air M2', 'Notebook ultrafino com chip M2', 10499.90, 1, 12),
	  (NEWID(), 'Headset HyperX Cloud II', 'Fone de ouvido gamer com som surround 7.1', 599.90, 1, 20),
	  (NEWID(), 'SSD NVMe Samsung 1TB', 'SSD de alta velocidade com leitura de 7000MB/s', 799.90, 1, 50),

	  -- Roupas (2)
	  (NEWID(), 'Tênis Adidas Ultraboost', 'Tênis esportivo confortável e moderno', 799.90, 2, 25),
	  (NEWID(), 'Boné New Era NY', 'Boné preto com logo NY bordado', 149.90, 2, 40),
	  (NEWID(), 'Calça Jeans Levi’s 501', 'Calça jeans clássica e resistente', 299.90, 2, 18),

	  -- Comida (3)
	  (NEWID(), 'Café Gourmet 500g', 'Café especial 100% arábica', 49.90, 3, 100),
	  (NEWID(), 'Pizza Congelada Pepperoni', 'Pizza pronta com queijo e pepperoni', 24.90, 3, 80),
	  (NEWID(), 'Vinho Tinto Cabernet Sauvignon', 'Vinho chileno de excelente safra', 89.90, 3, 50),

	  -- Móveis (4)
	  (NEWID(), 'Escrivaninha em Madeira', 'Escrivaninha moderna com gavetas', 1199.90, 4, 5),
	  (NEWID(), 'Sofá 3 Lugares Reclinável', 'Sofá confortável em couro sintético', 2599.90, 4, 3),
	  (NEWID(), 'Guarda-Roupa 6 Portas', 'Móvel espaçoso para armazenamento', 1899.90, 4, 6),

	  -- Livros (5)
	  (NEWID(), 'Livro O Poder do Hábito', 'Best-seller sobre mudança de comportamento', 89.90, 5, 20),
	  (NEWID(), 'Livro Sapiens', 'Uma breve história da humanidade', 99.90, 5, 15),
	  (NEWID(), 'Harry Potter e a Pedra Filosofal', 'Primeiro livro da saga de Harry Potter', 59.90, 5, 25),

	  -- Brinquedos (6)
	  (NEWID(), 'Boneca Barbie Fashionista', 'Boneca Barbie com roupa estilosa', 149.90, 6, 30),
	  (NEWID(), 'Carrinho Hot Wheels Pacote 5 Unidades', 'Miniaturas de carros esportivos', 79.90, 6, 50),
	  (NEWID(), 'Quebra-cabeça 1000 peças', 'Desafio para amantes de puzzles', 129.90, 6, 10),

	  -- Beleza (7)
	  (NEWID(), 'Base Líquida Maybelline Fit Me', 'Maquiagem de cobertura natural', 59.90, 7, 35),
	  (NEWID(), 'Creme Hidratante Nivea', 'Hidratação intensa para pele', 39.90, 7, 60),
	  (NEWID(), 'Máscara de Cílios Volume Express', 'Realça e alonga os cílios', 49.90, 7, 40),

	  -- Esportes (8)
	  (NEWID(), 'Bicicleta Speed Caloi', 'Bicicleta para alta performance', 4999.90, 8, 4),
	  (NEWID(), 'Halteres Ajustáveis 10kg', 'Par de halteres para treino em casa', 199.90, 8, 15),
	  (NEWID(), 'Corda de Pular Profissional', 'Equipamento para treino de cardio', 79.90, 8, 30),

	  -- Automotivo (9)
	  (NEWID(), 'Pneu Michelin 195/55 R16', 'Pneu para carros de passeio', 549.90, 9, 20),
	  (NEWID(), 'Kit Palhetas Limpador de Para-brisa', 'Jogo de palhetas de silicone', 79.90, 9, 50),
	  (NEWID(), 'Câmera de Ré Veicular', 'Facilita manobras em estacionamentos', 199.90, 9, 10),

	  -- Saúde (10)
	  (NEWID(), 'Medidor de Pressão Digital', 'Aparelho para monitoramento de pressão arterial', 179.90, 10, 15),
	  (NEWID(), 'Máscara N95 PFF2', 'Máscara de proteção respiratória', 19.90, 10, 100),
	  (NEWID(), 'Kit de Primeiros Socorros', 'Estojo completo para emergências', 129.90, 10, 20);
END