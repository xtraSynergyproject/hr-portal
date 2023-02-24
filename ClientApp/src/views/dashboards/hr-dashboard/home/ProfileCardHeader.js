// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Button from '@mui/material/Button'
import Avatar from '@mui/material/Avatar'
import CardMedia from '@mui/material/CardMedia'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import AvatarGroup from '@mui/material/AvatarGroup'
import Stepper from './Stepper'

const CardUser = () => {
    return (
        <Card sx={{ position: 'relative'}}>
            <CardMedia ><Stepper sx={{ height: '12.625rem' }} />  </CardMedia>

            <CardContent>
                <Box
                    sx={{
                        m: 'auto',
                        width: '80%',
                        display: 'flex',
                        flexWrap: 'wrap',
                        alignItems: 'center',
                        justifyContent: 'space-between',
                       
                    }}
                >
                    <Box sx={{ mr: 4, ms: 5, display: 'flex', flexDirection: 'column' }}>
                        <Avatar
                            alt='Robert Meyer'
                            src='/images/avatars/1.png'
                            sx={{
                                width: 150,
                                height: 150,
                                mt: '20%',
                                ml: '9%',
                                top: '7.28125rem',
                                position: 'absolute',
                                border: theme => `0.25rem solid ${theme.palette.common.white}`
                            }}
                        />

                        <Box sx={{ pl: 2, mt:4 }}>
                            <Typography variant='h6'>Administrator</Typography>
                            <Typography variant='p' >System Administrator</Typography>

                            <AvatarGroup max={4} sx={{mt:3}}>
                                <Avatar src='/images/avatars/8.png' alt='Alice Cobb' />
                                <Avatar src='/images/avatars/7.png' alt='Jeffery Warner' />
                                <Avatar src='/images/avatars/3.png' alt='Howard Lloyd' />
                                <Avatar src='/images/avatars/2.png' alt='Bettie Dunn' />
                                <Avatar src='/images/avatars/4.png' alt='Olivia Sparks' />
                                <Avatar src='/images/avatars/5.png' alt='Jimmy Hanson' />
                                <Avatar src='/images/avatars/6.png' alt='Hallie Richards' />
                            </AvatarGroup>
                        </Box>
                    </Box>

                    <Box sx={{display: 'flex', justifyContent: 'space-between', m:3}}>
                    <Button variant='contained'sx={{m:1}}> +Add to Story</Button>
                    <Button variant='contained'sx={{m:1}}> Edit Ptofile</Button>
                    </Box>
                </Box>
                <Box sx={{ gap: 2, display: 'flex', flexWrap: 'wrap', justifyContent: 'space-between', alignItems: 'center' }}>
                    <Typography variant='subtitle2' sx={{ whiteSpace: 'nowrap', color: 'text.primary' }}>
                        {/* 18 mutual friends */}
                    </Typography>

                </Box>
            </CardContent>
        </Card>
    )
}

export default CardUser
