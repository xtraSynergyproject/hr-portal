import * as React from 'react';
import { styled } from '@mui/material/styles';
import { Box, Paper, Grid } from '@mui/material';
//===

import { Card, CardContent, CardMedia, Typography } from '@mui/material';
import { CardActionArea } from '@mui/material';
//===

const Item = styled(Paper)(({ theme }) => ({
    backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
    ...theme.typography.body2,
    padding: theme.spacing(1),
    textAlign: 'center',
    color: theme.palette.text.secondary,
}));

export default function Cards() {
    return (
        <Box sx={{ flexGrow: 1 }}>
            <Grid container spacing={2}>
                <Grid item xs={3}>
                    <Item>
                        <Card sx={{ maxWidth: 300, margin: 'auto' }}>
                            <CardActionArea>
                                <CardMedia
                                    component="img"
                                    height="140"
                                    image="/static/images/cards/contemplative-reptile.jpg"
                                    alt="green iguana"
                                />
                                <CardContent>
                                    <Typography gutterBottom variant="h6" component="div">
                                       Leave Request
                                    </Typography>
                                   
                                </CardContent>
                            </CardActionArea>
                        </Card>
                    </Item>
                </Grid>
                <Grid item xs={3}>
                    <Item>
                        <Card sx={{ maxWidth: 300, margin: 'auto' }}>
                            <CardActionArea>
                                <CardMedia
                                    component="img"
                                    height="140"
                                    image="/static/images/cards/contemplative-reptile.jpg"
                                    alt="green iguana"
                                />
                                <CardContent>
                                    <Typography gutterBottom variant="h6" component="div">
                                      Request Time-Off
                                    </Typography>
                                 
                                </CardContent>
                            </CardActionArea>
                        </Card>
                    </Item>
                </Grid>
                <Grid item xs={3}>
                    <Item>
                        <Card sx={{ maxWidth: 300, margin: 'auto' }}>
                            <CardActionArea>
                                <CardMedia
                                    component="img"
                                    height="140"
                                    image="/static/images/cards/contemplative-reptile.jpg"
                                    alt="green iguana"
                                />
                                <CardContent>
                                    <Typography gutterBottom variant="h6" component="div">
                                      Employee Directory
                                    </Typography>
                                   
                                </CardContent>
                            </CardActionArea>
                        </Card>
                    </Item>
                </Grid>
                <Grid item xs={3}>
                    <Item>
                        <Card sx={{ maxWidth: 300, margin: 'auto' }}>
                            <CardActionArea>
                                <CardMedia
                                    component="img"
                                    height="140"
                                    image="/static/images/cards/contemplative-reptile.jpg"
                                    alt="green iguana"
                                />
                                <CardContent>
                                    <Typography gutterBottom variant="h6" component="div">
                                        My Profile
                                    </Typography>
                                   
                                </CardContent>
                            </CardActionArea>
                        </Card>
                    </Item>
                </Grid>
            </Grid>
        </Box>
    );
}