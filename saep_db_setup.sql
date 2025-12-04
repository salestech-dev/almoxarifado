

CREATE DATABASE saep_db
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;


USE saep_db;


CREATE TABLE produtos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(200) NOT NULL,
    quantidade INT NOT NULL DEFAULT 0,
    dataEntrada DATETIME NOT NULL,
    peso VARCHAR(50) NOT NULL,
    CONSTRAINT chk_quantidade_positiva CHECK (quantidade >= 0),
    INDEX idx_nome (nome),
    INDEX idx_dataEntrada (dataEntrada)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT INTO produtos (nome, quantidade, dataEntrada, peso) VALUES
('Caneta Azul BIC', 150, '2024-11-01 08:30:00', '0.010 kg'),
('Caneta Preta BIC', 120, '2024-11-01 08:30:00', '0.010 kg'),
('Lápis HB Faber-Castell', 200, '2024-11-05 09:15:00', '0.005 kg'),
('Borracha Branca', 180, '2024-11-05 09:15:00', '0.020 kg'),
('Caderno 100 folhas', 50, '2024-11-10 10:00:00', '0.300 kg'),
('Resma Papel A4', 100, '2024-11-12 14:20:00', '2.500 kg');


INSERT INTO produtos (nome, quantidade, dataEntrada, peso) VALUES
('Detergente Líquido 500ml', 80, '2024-11-15 11:00:00', '0.550 kg'),
('Desinfetante 1L', 60, '2024-11-15 11:00:00', '1.100 kg'),
('Sabão em Pó 1kg', 45, '2024-11-18 13:30:00', '1.000 kg'),
('Papel Higiênico (pacote 12 rolos)', 30, '2024-11-20 08:45:00', '0.800 kg'),
('Álcool Gel 70% 500ml', 95, '2024-11-22 09:00:00', '0.550 kg');


INSERT INTO produtos (nome, quantidade, dataEntrada, peso) VALUES
('Parafusos 5x25mm (caixa 100un)', 40, '2024-11-25 10:30:00', '0.200 kg'),
('Pregos 18x30mm (kg)', 25, '2024-11-25 10:30:00', '1.000 kg'),
('Fita Isolante Preta', 70, '2024-11-28 15:00:00', '0.050 kg'),
('Cola Instantânea 3g', 90, '2024-11-28 15:00:00', '0.010 kg');

INSERT INTO produtos (nome, quantidade, dataEntrada, peso) VALUES
('Luvas de Proteção (par)', 60, '2024-12-01 08:00:00', '0.150 kg'),
('Máscara PFF2 (unidade)', 150, '2024-12-01 08:00:00', '0.020 kg'),
('Óculos de Proteção', 35, '2024-12-02 09:30:00', '0.080 kg'),
('Capacete de Segurança', 20, '2024-12-02 09:30:00', '0.400 kg');

INSERT INTO produtos (nome, quantidade, dataEntrada, peso) VALUES
('Chave de Fenda Phillips', 25, '2024-12-03 11:00:00', '0.120 kg'),
('Chave de Fenda Reta', 25, '2024-12-03 11:00:00', '0.120 kg'),
('Martelo de Borracha', 15, '2024-12-03 14:00:00', '0.350 kg'),
('Alicate Universal 8"', 18, '2024-12-04 08:30:00', '0.280 kg'),
('Trena 5 metros', 22, '2024-12-04 08:30:00', '0.200 kg');


CREATE VIEW vw_estoque_baixo AS
SELECT 
    id,
    nome,
    quantidade,
    dataEntrada,
    peso
FROM produtos
WHERE quantidade < 30
ORDER BY quantidade ASC;


CREATE VIEW vw_produtos_recentes AS
SELECT 
    id,
    nome,
    quantidade,
    dataEntrada,
    peso,
    DATEDIFF(NOW(), dataEntrada) AS dias_desde_entrada
FROM produtos
WHERE dataEntrada >= DATE_SUB(NOW(), INTERVAL 30 DAY)
ORDER BY dataEntrada DESC;


CREATE VIEW vw_resumo_estoque AS
SELECT 
    COUNT(*) AS total_produtos,
    SUM(quantidade) AS quantidade_total,
    MIN(dataEntrada) AS primeira_entrada,
    MAX(dataEntrada) AS ultima_entrada
FROM produtos;



DELIMITER //


CREATE PROCEDURE sp_adicionar_produto(
    IN p_nome VARCHAR(200),
    IN p_quantidade INT,
    IN p_dataEntrada DATETIME,
    IN p_peso VARCHAR(50)
)
BEGIN
    IF p_quantidade < 0 THEN
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'Quantidade não pode ser negativa';
    END IF;
    
    INSERT INTO produtos (nome, quantidade, dataEntrada, peso)
    VALUES (p_nome, p_quantidade, p_dataEntrada, p_peso);
    
    SELECT LAST_INSERT_ID() AS novo_id;
END //
