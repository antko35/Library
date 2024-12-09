import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import { Button, Card, Typography, Input, Form, Spin, Row, Col, Select } from 'antd';
import useFetchBook from '../../hooks/useFetchBook';
import useAuthors from '../../hooks/useAuthors';
import useGenres from '../../hooks/useGenres';
import { Center } from '@chakra-ui/react';
import Comments from '../../сomponents/Comments/Comments';

const { Title, Paragraph } = Typography;
const { Option } = Select;

const BookPage = ({ isAdmin }) => {
  const { id } = useParams();
  const { book, loading, error, setBook, loadBookData } = useFetchBook(id);
  const [isEditing, setIsEditing] = useState(false);
  const [form] = Form.useForm();
  const { genres, loading: loadingGenres, error: errorGenres } = useGenres();
  const { authors, loading: loadingAuthors, error: errorAuthors } = useAuthors();

  const handleEdit = () => {
    setIsEditing(true);
    form.setFieldsValue(book); // Заполняем форму текущими данными книги
  };

  const handleSave = async (values) => {

    const updatedValues = {
      id: book.id,
      isbn: book.isbn,
      ...values, // Остальные поля добавляются после id и isbn
    };
    // Сохранение изменений на сервере
    try {
      await fetch(`https://localhost:7040/Books/update`, {
        method: 'PUT',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(updatedValues),
      });
      console.log(updatedValues);

      //setBook(values); // Обновляем данные о книге
      loadBookData();
      setIsEditing(false);
    } catch (error) {
      console.error('Ошибка при сохранении изменений:', error);
    }
  };

  if (loading) return <Spin tip="Loading ..." />;
  if (error) return <p>{error}</p>;

  return (
    <Card>
      {isEditing ? (
        <Form form={form} onFinish={handleSave} layout="vertical">
          <Form.Item name="title" label="Title">
            <Input />
          </Form.Item>
          <Form.Item name="authorId" label="Author">
            <Select placeholder="Select an author">
              {authors.map(author => (
                <Option key={author.id} value={author.id}>
                  {`${author.name} ${author.surname}`}
                </Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="genreId" label="Genre">
            <Select placeholder="Select genre">
              {genres.map(genre => (
                <Option key={genre.id} value={genre.id}>
                  {`${genre.genre}`}
                </Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="description" label="Description">
            <Input.TextArea rows={4} />
          </Form.Item>
          <Button type="primary" htmlType="submit">Save</Button>
          <Button onClick={() => setIsEditing(false)} style={{ marginLeft: 8 }}>Cancle</Button>
        </Form>
      ) : (
        <Row gutter={16} >
          <Col xs={24} sm={8} md={6} style={{ display: 'flex', justifyContent: 'center' }} >
            {book.coverImagePath ? (
              <img
                src={`https://localhost:7040/uploads/${book.coverImagePath}`}
                alt="Book cover"
                style={{ width: '50%', height: 'auto', borderRadius: '4px' }}
              />
            ) : (
              <div
                style={{
                  width: '50%',
                  height: '100%',
                  backgroundColor: '#f0f0f0',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  borderRadius: '4px',
                  padding: '16px',
                  textAlign: 'center',
                }}
              >
                No photo
              </div>
            )}
          </Col>
          <Col xs={24} sm={16} md={18}>
            <Title level={2}>{book.title}</Title>
            <Paragraph>Author: {book.author?.name + " " + book.author?.surname}</Paragraph>
            <Paragraph>Genre: {book.genre?.genre}</Paragraph>
            <Paragraph>ISBN: {book.isbn}</Paragraph> 
            <Paragraph>Description: {book.description}</Paragraph>
            {isAdmin && (
              <Button type="primary" onClick={handleEdit} style={{ marginTop: 16 }}>
                Edit
              </Button>
            )}
          </Col>
        </Row>
      )}
      <Comments bookId={book.id} />
    </Card>
  );
};

export default BookPage;
