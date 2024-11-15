import React, { useEffect, useState } from 'react';
import { Modal, Button, List, Input, Spin, message } from 'antd';

const AddGenre = ({ visible, onClose }) => {
  const [genres, setGenres] = useState([]);
  const [loading, setLoading] = useState(false);
  const [newGenreName, setNewGenreName] = useState('');

  const fetchGenres = async () => {
    setLoading(true);
    try {
      const response = await fetch('https://localhost:7040/Genre', {
        method: 'GET',
        credentials: 'include',
      });
      const data = await response.json();
      setGenres(data);
    } catch (error) {
      console.error('Error fetching genres:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (genreId) => {
    Modal.confirm({
      title: 'Are you shure ?',
      content: 'This action cannot be undone.',
      okText: 'Yes',
      okType: 'danger',
      cancelText: 'No',
      onOk: async () => {
        try {
          await fetch(`https://localhost:7040/Genre/${genreId}`, {
            method: 'DELETE',
            credentials: 'include',
          });
          fetchGenres(); // Обновить список жанров после удаления
        } catch (error) {
          console.error('Error deleting genre:', error);
        }
      },
    });
  };

  const handleAddGenre = async () => {
    // Проверка на пустое поле
    if (!newGenreName) {
      message.error('Please, enter genre name.'); // Отображаем сообщение об ошибке
      return;
    }

    try {
      await fetch('https://localhost:7040/Genre', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Genre: newGenreName }), // Отправляем только название жанра
        credentials: 'include',
      });
      fetchGenres(); // Обновить список жанров после добавления
      setNewGenreName(''); // Сбрасываем поле ввода
    } catch (error) {
      console.error('Error adding genre:', error);
      message.error('Error while adding genre'); // Сообщение об ошибке при добавлении
    }
  };

  useEffect(() => {
    if (visible) {
      fetchGenres();
    }
  }, [visible]);

  return (
    <Modal
      title="Manage Genres"
      visible={visible}
      onCancel={onClose}
      footer={null}
    >
      {loading ? (
        <Spin tip="Loading genres..." />
      ) : (
        <>
          <Input
            placeholder="Genre name"
            value={newGenreName}
            onChange={(e) => setNewGenreName(e.target.value)}
            style={{ marginBottom: '10px' }}
          />
          <Button type="primary" onClick={handleAddGenre} style={{ marginBottom: '20px' }}>
            Add Genre
          </Button>
          <div style={{ maxHeight: 200, overflowY: 'auto' }}>
            <List
              bordered
              dataSource={genres}
              renderItem={(genre) => (
                <List.Item
                  actions={[
                    <Button type="link" danger onClick={() => handleDelete(genre.id)}>
                      Delete
                    </Button>,
                  ]}
                >
                  {genre.genre}
                </List.Item>
              )}
            />
          </div>
        </>
      )}
    </Modal>
  );
};

export default AddGenre;
