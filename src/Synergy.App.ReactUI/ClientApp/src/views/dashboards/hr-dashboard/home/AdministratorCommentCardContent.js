// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Collapse from '@mui/material/Collapse'
import IconButton from '@mui/material/IconButton'
import Typography from '@mui/material/Typography'
import CardHeader from '@mui/material/CardHeader'
import CardContent from '@mui/material/CardContent'

// ** Icon Imports
import Icon from 'src/@core/components/icon'
import AdministratorCommentCard from './AdministratorCommentCard'
import CommentsCard from './CommentsCard'
import CommentsSecondCard from './CommentsSecondCard'
import CommentThirdCard from './CommentsThirdCard'


const AdministratorCommentCardContent = () => {
    // ** State
    const [collapsed, setCollapsed] = useState(true)

    return (
        <Card>
            <CardHeader
                title='Comments'
                sx={{float:'left'}}
                action={
                    <IconButton
                        size='small'
                        aria-label='collapse'
                        sx={{ color: 'text.secondary' }}
                        onClick={() => setCollapsed(!collapsed)}
                    >
                        <Icon fontSize={20} icon={!collapsed ? 'mdi:chevron-down' : 'mdi:chevron-up'} />
                    </IconButton>
                }
            />
            <Collapse in={collapsed}>
                <CardContent>
                    <Typography variant='body2'>
                        <CommentsCard />
                        <Box component='span' sx={{ verticalAlign: 'top' }}>
                            <Icon icon='mdi:chevron-up' fontSize={20} />
                        </Box>{' '}
                       <CommentsSecondCard/>
                       <Box component='span' sx={{ verticalAlign: 'top' }}>
                            <Icon icon='mdi:chevron-up' fontSize={20} />
                        </Box>{' '}
                        <CommentThirdCard/>
                    </Typography>
                </CardContent>
            </Collapse>
        </Card>
    )
}

export default AdministratorCommentCardContent