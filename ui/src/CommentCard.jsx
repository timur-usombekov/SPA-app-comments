import React from 'react';
import { Card, Button, Icon } from '@blueprintjs/core';
import './App.css';

function CommentCard({ username, email, text, file }) {
    return (
        <Card interactive={false} elevation={2} className="commentCard">
            <div className="commentCard-header">
                <strong>{username}</strong>
                <span className="commentCard-email">({email})</span>
            </div>
            <p className="commentCard-text">{text}</p>
            {file && (
                <div className="commentCard-file">
                    <a href={URL.createObjectURL(file)} target="_blank" rel="noopener noreferrer">
                        <Icon icon="document" /> {file.name}
                    </a>
                </div>
            )}
            <div className="commentCard-actions">
                <Button intent="primary" text="Reply" />
                <Button intent="success" text="Watch replies" />
            </div>
        </Card>
    );
}

export default CommentCard;
