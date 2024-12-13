import React, { useState, useEffect } from "react";
import { Modal, Form, Input, Select, Button, Upload, message } from "antd";
import { UploadOutlined } from "@ant-design/icons";
import axios from "axios";
import { json } from "react-router-dom";

const { Option } = Select;

const AddBook = ({ genres, authors, visible, onClose, onCreate }) => {
  const [form] = Form.useForm();
  const [uploading, setUploading] = useState(false);
  const [imageUrl, setImageUrl] = useState(null);
  const [imageFile, setImageFile] = useState(null);

  // useEffect(() => {
  //   // Загрузка авторов и жанров с бэкенда
  //   const fetchData = async () => {
  //     try {
  //       const [authorsResponse, genresResponse] = await Promise.all([
  //         axios.get("https://localhost:7040/Author",{withCredentials : true}),
  //         axios.get("https://localhost:7040/Genre",{withCredentials : true}),
  //       ]);

  //       const formattedAuthors = authorsResponse.data.map((author) => ({
  //           id: author.id,
  //           fullName: `${author.name} ${author.surname}`,
  //         }));

  //       //setAuthors(formattedAuthors);
  //       //setGenres(genresResponse.data);
  //     } catch (error) {
  //       message.error("Ошибка загрузки авторов или жанров");
  //     }
  //   };

  //   fetchData();
  // }, []);


  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      const payload = {
        ...values,
      };

      var bookResponse = await onCreate(payload);
      var bookData = await bookResponse.json();
      console.log(payload);
      if (imageFile) {
        await handleUploadImage(imageFile, bookData.id); // Используем ID книги
      }

      form.resetFields();
      setImageUrl(null);
    } catch (error) {
      console.log(error)
      message.error(`Пожалуйста, заполните все обязательные поля.`);
    }
  };

  const handleUploadImage = async (file, bookId) => {
    const formData = new FormData();
    formData.append("file", file);

    setUploading(true);
    try {
      console.log("Uploading");
      await axios.post(`https://localhost:7040/Books/upload-cover/${bookId}`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
        withCredentials: true,
      });
      message.success("Обложка загружена успешно");
    } catch (error) {
      message.error("Ошибка загрузки обложки");
    } finally {
      setUploading(false);
    }
  };

  const handleFileChange = (info) => {
    if (info.file && info.file.originFileObj) {
      const objectUrl = URL.createObjectURL(info.file.originFileObj);
      setImageUrl(objectUrl); // Предпросмотр изображения
      setImageFile(info.file.originFileObj); // Сохранение файла для последующей загрузки
    }
  };

  return (
    <Modal
      visible={visible}
      title="Создать книгу"
      onCancel={onClose}
      footer={null}
    >
      <Form form={form} layout="vertical">

      <Form.Item
          name="isbn"
          label="ISBN"
          rules={[{ required: true, message: "Пожалуйста, введите ISBN" }]}
        >
          <Input placeholder="Введите ISBN" />
        </Form.Item>

        <Form.Item
          name="title"
          label="Название"
          rules={[{ required: true, message: "Пожалуйста, введите название книги" }]}
        >
          <Input placeholder="Введите название книги" />
        </Form.Item>

        <Form.Item
          name="authorId"
          label="Автор"
          rules={[{ required: true, message: "Пожалуйста, выберите автора" }]}
        >
          <Select placeholder="Выберите автора">
            {authors.map((author) => (
              <Option key={author.id} value={author.id}>
                { `${author.name} ${author.surname}`}
              </Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item
          name="genreId"
          label="Жанр"
          rules={[{ required: true, message: "Пожалуйста, выберите жанр" }]}
        >
          <Select placeholder="Выберите жанр">
            {genres.map((genre) => (
              <Option key={genre.id} value={genre.id}>
                {genre.genre}
              </Option>
            ))}
          </Select>
        </Form.Item>

        
        <Form.Item name="description" label="Описание" rules={[{ required: true, message: "Пожалуйста, введите описание" }]} >
          <Input.TextArea placeholder="Введите описание книги" rows={4} />
        </Form.Item>

        <Form.Item label="Обложка">
          <Upload
            customRequest={() => false}
            onChange={handleFileChange}
            showUploadList={true}
            accept="image/*"
          >
            <Button icon={<UploadOutlined />} loading={uploading}>
              Загрузить изображение
            </Button>
          </Upload>
          {imageUrl && <img src={imageUrl} alt="Обложка" style={{ marginTop: 10, maxHeight: 200 }} />}
        </Form.Item>

        <Form.Item>
          <Button type="primary" onClick={handleSubmit} block>
            Создать книгу
          </Button>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default AddBook;
