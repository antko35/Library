import React from 'react';
import { Card, Col, Row, Button} from 'antd'; 
import { useNavigate } from 'react-router-dom';

const BookCard = ({ id,title, author, genre, inProfile, coverImagePath, onBorrow }) => {
  const navigate = useNavigate();
  const availability = inProfile === false ? 'Доступна' : `В профиле`;
  const isAvailable = inProfile === false;

  const handleClick = () =>{
    navigate(`/book/${id}`);
  }

  return (
    <Card
      hoverable
      style={{ width: "30%" }}
      onClick={handleClick}>
      <Row gutter={16}>
        <Col span={8}>
          <img
            alt='cover'
            src={`https://localhost:7040/uploads/${coverImagePath}`}
            style={{ width: '100%', height: 'auto' }} />
        </Col>
        <Col span={16}>
          <Card.Meta title={title} description={`Автор: ${author}`} />
          <p>Жанр: {genre}</p>
          <p>Статус: {availability}</p>

          {isAvailable && (
            <Button 
              type="primary" 
              style={{marginTop: '10px'}}
              onClick= {(e) =>{
                e.stopPropagation();
                onBorrow();
                }}>
              Добавить в профиль
            </Button>
          )}
        </Col>
      </Row>
    </Card>
  );
};

export default BookCard;
