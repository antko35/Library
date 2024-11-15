import React from 'react';
import { Card, Col, Row, Button} from 'antd'; 
import { useNavigate } from 'react-router-dom';

const BookCard = ({ id,title, author, genre, returnDate, coverImagePath, onBorrow }) => {
  const navigate = useNavigate();
  const availability = returnDate === null ? 'Available' : `booked until ${returnDate}`;
  const isAvailable = returnDate === null;

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
          <Card.Meta title={title} description={`By ${author}`} />
          <p>Genre: {genre}</p>
          <p>Status: {availability}</p>

          {isAvailable && (
            <Button 
              type="primary" 
              onClick= {(e) =>{
                e.stopPropagation();
                onBorrow();
                }}>
              Add to Profile
            </Button>
          )}
        </Col>
      </Row>
    </Card>
  );
};

export default BookCard;
