// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import Grid from '@mui/material/Grid'



import Paper from '@mui/material/Paper';
import CardMedia from '@mui/material/CardMedia';
import Button from '@mui/material/Button';
import CompassionatelyLeave from '../Components/LeaveCard/CompassionatelyLeave'
import AdjustmentLeave from '../Components/LeaveCard/AdjustmentLeave'
import AnnualLeaveEncasement from '../Components/LeaveCard/AnnualLeaveEncasement'
import LeaveaAdjustment from '../Components/LeaveCard/LeaveAdjusment'
import LeaveCancel from '../Components/LeaveCard/LeaveCancel'


import MarriageLeave from '../Components/LeaveCard/MarriageLeave'
import MaternityLeave from '../Components/LeaveCard/MaternityLeave'
import PaternityLeave from '../Components/LeaveCard/PaternityLeave'

import ExaminationLeave from '../Components/LeaveCard/ExaminationLeave'
import UnderTimeLeave from '../Components/LeaveCard/UnderTimeLeave'
import SampleLeave from '../Components/LeaveCard/SampleLeave'
import SickLeave from '../Components/LeaveCard/SickLeave'
import LeaveHandOverService from '../Components/LeaveCard/LeaveHandOverService'
import UnpaidLeave from '../Components/LeaveCard/UnPaidLeave'
import AnnualLeave from '../Components/LeaveCard/AnnualLeave'


// Styled Grid component
const StyledGrid = styled(Grid)(({ theme }) => ({
    [theme.breakpoints.down('sm')]: {
        display: 'flex',
        justifyContent: 'center'
    }
}))

// Styled component for the image
const Img = styled('img')(({ theme }) => ({
    right: 13,
    bottom: 0,
    height: '255',
    position: 'absolute',
    [theme.breakpoints.down('sm')]: {
        height: 165,
        position: 'static'
    }
}))


const Item = styled(Paper)(({ theme }) => ({
    backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
    ...theme.typography.body2,
    padding: theme.spacing(1),
    textAlign: 'center',
    color: theme.palette.text.secondary,
}));





const CardWelcomeBack = () => {
    return (


        <Box sx={{ height: 820, width: 999, background: 'white', transform: 'translateZ(0px)', flexGrow: 1 }}>




            <Card sx={{ position: 'relative', overflow: 'visible', mt: { xs: 0, sm: 14.4, md: 0 } }}>
                <CardContent sx={{ p: theme => theme.spacing(7.25, 7.5, 7.75, 7.5) }}>


                    <Grid container rowSpacing={4} columnSpacing={20}>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://th.bing.com/th/id/OIP.LciTNhZjw8eFQOLLtG4mawHaGG?pid=ImgDet&rs=1"
                                    title="green iguana"

                                />
                                <CompassionatelyLeave />


                            </Item>
                        </Grid>

                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://th.bing.com/th/id/OIP.BvzRC46VL0qmmwlbIA0WtAHaE7?w=254&h=180&c=7&r=0&o=5&pid=1.7"
                                    title="green iguana"

                                />
                                <AdjustmentLeave />


                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://th.bing.com/th/id/OIP.Srv_C1WBdLgMg39gyRfxDgHaD4?w=282&h=180&c=7&r=0&o=5&pid=1.7"
                                    title="green iguana"
                                />
                                <LeaveaAdjustment />

                            </Item>
                        </Grid>

                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://www.signaturestaff.com.au/wp-content/uploads/2016/07/leave-cancellation.jpg"
                                    title="green iguana"
                                />
                                <LeaveCancel />

                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://th.bing.com/th/id/OIP.7eTnY5DuuU1aNyJcCzyavQHaFj?pid=ImgDet&w=736&h=552&rs=1"
                                    title="green iguana"
                                />
                                <MarriageLeave />

                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://th.bing.com/th/id/OIP.0Qtyk58q8SMzAu08QaBkTgHaFb?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <PaternityLeave />

                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://2.bp.blogspot.com/-6CMKoesdwZU/WgCoAzf3VdI/AAAAAAAAMqw/c3u_t2nJctUJCm7rYGG_TzLbBmv44cr7ACLcBGAs/s1600/Maternity2017.jpg"
                                    title="green iguana"
                                />
                                <MaternityLeave />

                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://i.pinimg.com/736x/8b/1a/d8/8b1ad84e055b80262af92696b23fb116.jpg"
                                    title="green iguana"
                                />

                                <AnnualLeaveEncasement />

                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://th.bing.com/th/id/OIP.0Y8fmMTjCc2L9YucYXQqKQHaFa?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <SickLeave />

                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://th.bing.com/th/id/OIP.ZQC7DRRbK-_Ma56FwC9K1gHaE8?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <UnpaidLeave />

                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://blogs.glowscotland.org.uk/nl/public/CardinalNewmanWebsite/uploads/sites/21883/2017/05/study-leave.jpg"
                                    title="green iguana"
                                />
                                <ExaminationLeave />

                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://th.bing.com/th/id/OIP.4i3AQu4B7vDF9-WSPd1-mQHaFP?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <LeaveHandOverService />

                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://th.bing.com/th/id/OIP.SiC4WD4-GF10pxH-gaqdvgHaHa?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <AnnualLeave />

                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://i.ytimg.com/vi/c8NulVkRsHI/maxresdefault.jpg"
                                    title="green iguana"
                                />
                                <SampleLeave />




                            </Item>
                        </Grid>
                        <Grid item xs={4}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: '255' }}
                                    image="https://thumbs.dreamstime.com/b/business-man-doing-hard-work-office-22765456.jpg"
                                    title="green iguana"
                                />
                                <UnderTimeLeave />




                            </Item>
                        </Grid>



                    </Grid>


                </CardContent>
            </Card>
        </Box>
    )
}

export default CardWelcomeBack
